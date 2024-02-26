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
using Am.Application.Helper;
using AM.Domain.Dto;
using AM.Application.Helper;

using Am.Persistence.Seeding;
using System.Text;
using System.Text.RegularExpressions;

namespace SKD_Automation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<User> _validator;

        public LoginController(IUnitWorkService service, IMapper mapper, IValidator<User> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginDto obj)
        {
            if (COM.IsNull(obj)) return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "Login details can't be null" });

            User user = await _service.User.GetFirstOrDefault(e => e.UserName.Equals(obj.UserName), noTracking: false, includeProp: $"{nameof(AM.Domain.Entities.User.Role)}");
            IEnumerable<User> logins = await _service.User.GetAll();

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

            User user = await _service.User
                .GetFirstOrDefault(e => e.UserName.Equals(username), noTracking: false, includeProp: $"{nameof(AM.Domain.Entities.User.Role)}");

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) return BadRequest(new ApiError
            {
                ErrorCode = 400,
                ErrorMessage = "Invalid client request!"
            });

            IEnumerable<User> users = await _service.User.GetAll();

            var newAccessToken = TokenHelper.CreateJWTToken(user);
            var newRefreshToken = TokenHelper.CreateRefreshToken(users);

            user.RefreshToken = newRefreshToken;

            await _service.Commit();

            return Ok(new TokenApiDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
