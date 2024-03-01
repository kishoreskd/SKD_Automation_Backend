using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AM.Domain.Entities;
using AM.Persistence;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using AM.Domain.Dto;
using System.Linq;

using Am.Persistence.Seeding;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using AM.Application.Exceptions;
using System.Net;
using SKD_Automation.Helper;
using SKD_Automation.Filters;

namespace SKD_Automation.Controllers
{
    [ServiceFilter(typeof(HeaderAuthorizationFilterForLogin))]
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<User> _validator;

        public UserController(IUnitWorkService service, IMapper mapper, IValidator<User> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }


        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<User> users = await _service.User.GetAll(includeProp: $"{nameof(AM.Domain.Entities.Role)}");
            IEnumerable<UserDto> dtos = users.Select(e => _mapper.Map<UserDto>(e));

            if (!COM.IsAny(dtos))
            {
                return NotFound();
            }

            return Ok(dtos);
        }

        [HttpPost("post/register")]
        public async Task<IActionResult> Register(UserDto dto)
        {
            User usr = _mapper.Map<User>(dto);

            ValidationResult result = _validator.Validate(usr);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            if (await CheckUserNameExistAsync(usr.UserName))
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = 400,
                    ErrorMessage = "User Name already exist!"
                });
            }

            string passMsg = PasswordHelper.CheckPasswordStrength(usr.Password);

            if (!COM.IsNullOrEmpty(passMsg)) return BadRequest(new ApiError
            {
                ErrorCode = 400,
                ErrorMessage = passMsg
            });

            usr.Password = PasswordHelper.HashPassword(usr.Password);
            usr.CreatedBy = dto.CreatedBy;
            usr.CreatedDate = dto.CreatedDate;

            await _service.User.Add(usr);
            await _service.Commit();

            return Ok(usr);
        }

        [HttpPut("put/register/{userId}")]
        public async Task<IActionResult> Update(int userId, UserDto dto)
        {
            User existinguser = await _service.User.GetFirstOrDefault(e => e.Id.Equals(userId), noTracking: false);
            string existingUserName = existinguser.UserName;
            User usr = _mapper.Map(dto, existinguser);

            if (COM.IsNull(usr))
            {
                return NotFound();
            }

            ValidationResult result = _validator.Validate(usr);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            if (!existingUserName.Equals(usr.UserName))
            {
                if (await CheckUserNameExistAsync(usr.UserName))
                {
                    return BadRequest(new ApiError
                    {
                        ErrorCode = 400,
                        ErrorMessage = "User Name already exist!"
                    });
                }
            }

            string passMsg = PasswordHelper.CheckPasswordStrength(usr.Password);

            if (!COM.IsNullOrEmpty(passMsg)) return BadRequest(new ApiError
            {
                ErrorCode = 400,
                ErrorMessage = passMsg
            });

            usr.Password = PasswordHelper.HashPassword(usr.Password);

            usr.LastModifiedBy = dto.LastModifiedBy;
            usr.LastModifiedDate = dto.LastModifiedDate;

            await _service.Commit();

            return Ok(usr);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeletePlugin(int id)
        {
            User user = await _service.User.GetFirstOrDefault(e => e.Id.Equals(id));

            if (COM.IsNull(user))
            {
                return NotFound();
            }

            bool plugin = await _service.Plugin.IsAnyAsync(e => e.CreatedBy.Equals(user.EmployeeId));
            bool pluginLog = await _service.PluginLog.IsAnyAsync(e => e.CreatedBy.Equals(user.EmployeeId));

            if (plugin)
            {
                //return Content(HttpStatusCode.Conflict, "");

                throw new DeleteFailureException("User", user.EmployeeId, "There are user are associated with plugins!");
            }

            if (pluginLog)
            {
                throw new DeleteFailureException("User", user.EmployeeId, "There are user are associated with plugin log!");
            }

            _service.User.Remove(user);
            await _service.Commit();
            return StatusCode(200);
        }

        private async Task<bool> CheckUserNameExistAsync(string userName) => await _service.User.IsAnyAsync(e => e.UserName.Equals(userName));
    }
}

