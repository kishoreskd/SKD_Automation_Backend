using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Diagnostics;

namespace AM.Application.Helper
{
    public class TokenHelper
    {
        public static string CreateJWTToken(Login login)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("PGTINDI@345");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, login.Role.RoleName),
                new Claim(ClaimTypes.Name,$"{login.EmployeeId}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(10),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public static string CreateRefreshToken(IEnumerable<Login> logins)
        {
            byte[] tokenBytes = new byte[64];
            RandomNumberGenerator.Create().GetBytes(tokenBytes);

            var refreshToken = Convert.ToBase64String(tokenBytes);

            if (logins.Any(e => e.RefreshToken.Equals(refreshToken)))
            {
                return CreateRefreshToken(logins);
            }

            return refreshToken;
        }

        public static ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("PGTINDI@345");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");

            return principal;
        }
    }
}
