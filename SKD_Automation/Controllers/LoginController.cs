using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using AM.Domain.Entities;
using AM.Persistence;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using AM.Domain.Dto;

using Am.Persistence.Seeding;
using System.Text;
using System.Text.RegularExpressions;
using SKD_Automation.Helper;
using Microsoft.EntityFrameworkCore;

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

            IQueryable<User> logins = _service.User.GetIQueryable();

            if (COM.IsNull(user)) return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "User name does not match!" });

            if (!PasswordHelper.VerifyPassword(obj.Password, user.Password)) return BadRequest(new ApiError { ErrorCode = 400, ErrorMessage = "Password is incorrect!" });

            user.Token = TokenHelper.CreateJWTToken(user);
            var newAccessToken = user.Token;
            var newRefreshToken = TokenHelper.CreateRefreshToken(logins);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(10);
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
            try
            {
                string accessToken = tokenApiDto.AccessToken;
                string refreshToken = tokenApiDto.RefreshToken;


                if (tokenApiDto is null) return BadRequest(new ApiError
                {
                    ErrorCode = 400,
                    ErrorMessage = "Invalid client request!"
                });

                var principal = TokenHelper.GetPricipalForRefreshToken(accessToken);

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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
