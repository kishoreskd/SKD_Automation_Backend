using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using AM.Domain.Entities;
using AM.Persistence;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Am.Persistence.Seeding;
using System.Text;
using System.Text.RegularExpressions;
using Am.Application.Helper;
using AM.Domain.Dto;
using AM.Application.Helper;

namespace SKD_Automation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<Login> _validator;

        public LoginController(IUnitWorkService service, IMapper mapper, IValidator<Login> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(Login obj)
        {
            if (COM.IsNull(obj)) return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "Login details can't be null" });

            Login user = await _service.Login.GetFirstOrDefault(e => e.EmployeeId.Equals(obj.EmployeeId), noTracking: false);
            IEnumerable<Login> logins = await _service.Login.GetAll();

            if (COM.IsNull(user)) return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "User not found!" });

            if (!PasswordHelper.VerifyPassword(obj.Password, user.Password)) return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "Password is incorrect!" });

            user.Token = TokenHelper.CreateJWTToken(user);
            var newAccessToken = user.Token;
            var newRefreshToken = TokenHelper.CreateRefreshToken(logins);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            await _service.Commit();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("post/register")]
        public async Task<IActionResult> Register(Login obj)
        {
            ValidationResult result = _validator.Validate(obj);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            if (await CheckUserNameExistAsync(obj.EmployeeId))
            {
                return BadRequest(new ApiError
                {
                    ErrorCode = 400,
                    ErrorMessage = "User Name already exist!"
                });
            }

            string passMsg = PasswordHelper.HashPassword(obj.Password);

            if (!COM.IsNullOrEmpty(passMsg)) return BadRequest(new ApiError
            {
                ErrorCode = 400,
                ErrorMessage = passMsg
            });

            obj.Password = PasswordHelper.HashPassword(obj.Password);

            await _service.Login.Add(obj);
            await _service.Commit();

            return Ok();
        }

        [HttpPost("token/refresh")]
        public async Task<IActionResult> Refresh(TokenApiDto tokenApiDto)
        {
            if (tokenApiDto is null) return BadRequest(new ApiError
            {
                ErrorCode = 400,
                ErrorMessage = "Invalid client request!"
            });

            string accessToken = tokenApiDto.AccessToken;
            string refreshToken = tokenApiDto.RefreshToken;

            var principal = TokenHelper.GetPrincipleFromExpiredToken(accessToken);
            string username = principal.Identity.Name;

            Login user = await _service.Login.GetFirstOrDefault(e => e.EmployeeId.Equals(Convert.ToInt32(username)), noTracking: false);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) return BadRequest(new ApiError
            {
                ErrorCode = 400,
                ErrorMessage = "Invalid client request!"
            });

            IEnumerable<Login> logins = await _service.Login.GetAll();
            var newAccessToken = TokenHelper.CreateJWTToken(user);
            var newRefreshToken = TokenHelper.CreateRefreshToken(logins);

            user.RefreshToken = newRefreshToken;

            await _service.Commit();

            return Ok(new TokenApiDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        public async Task<bool> CheckUserNameExistAsync(int userName) => await _service.Login.IsAnyAsync(e => e.EmployeeId.Equals(userName));
    }
}
