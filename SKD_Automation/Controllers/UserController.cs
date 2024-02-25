﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AM.Domain.Entities;
using AM.Persistence;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Am.Application.Helper;
using AM.Domain.Dto;
using AM.Application.Helper;
using System.Linq;

using Am.Persistence.Seeding;
using System.Text;
using System.Text.RegularExpressions;

namespace SKD_Automation.Controllers
{
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

        [HttpPut("put/register")]
        public async Task<IActionResult> Update(int userId, UserDto dto)
        {
            User existinguser = await _service.User.GetFirstOrDefault(e => e.Id.Equals(userId), noTracking: false);
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

            usr.LastModifiedBy = dto.LastModifiedBy;
            usr.LastModifiedDate = dto.LastModifiedDate;

            await _service.Commit();

            return Ok(usr);
        }

        private async Task<bool> CheckUserNameExistAsync(string userName) => await _service.User.IsAnyAsync(e => e.UserName.Equals(userName));
    }
}