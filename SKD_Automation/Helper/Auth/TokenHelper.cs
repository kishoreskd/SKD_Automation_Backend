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
using System.Configuration;
using Microsoft.Extensions.Configuration;
using AM.Persistence;
using AM.Application.Exceptions;

namespace SKD_Automation.Helper
{
    public static class TokenHelper
    {
        #region Login TOKEN

        public static string CreateJWTToken(User user)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM");

                var identity = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(type: "employeeId", value: $"{user.EmployeeId}")
                });

                var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    Expires = DateTime.Now.AddHours(1),
                    //Expires = DateTime.Now.AddSeconds(1),

                    SigningCredentials = credentials
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                return jwtTokenHandler.WriteToken(token);
            }
            catch
            {
                throw new JWTExcpetion("Token creation failed!");
            }
        }
        public static string CreateRefreshToken(IEnumerable<User> users)
        {
            try
            {
                byte[] tokenBytes = new byte[64];
                RandomNumberGenerator.Create().GetBytes(tokenBytes);

                var refreshToken = Convert.ToBase64String(tokenBytes);

                if (users.Any(e => e.RefreshToken != null && e.RefreshToken.Equals(refreshToken)))
                {
                    return CreateRefreshToken(users);
                }

                return refreshToken;
            }
            catch (Exception ex)
            {
                throw new JWTExcpetion("Unauthorized - Token is invalid.");
            }
        }
        public static ClaimsPrincipal GetPricipalForRefreshToken(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM");

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false,
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("This is Invalid Token");

                return principal;
            }
            catch (Exception ex)
            {
                throw new JWTExcpetion("Unauthorized - Token is invalid.");
            }
        }
        public static ClaimsPrincipal GetPrincipalForLgn(string token)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM");

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;


                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("This is Invalid Token");

                if (!jwtSecurityToken.Payload.ContainsKey("exp"))
                {
                    throw new SecurityTokenExpiredException();
                }

                double expirationTimeStamp = Convert.ToDouble(jwtSecurityToken.Payload["exp"]);
                var expDateTimeUtc = DateTimeOffset.FromUnixTimeSeconds((long)expirationTimeStamp).UtcDateTime;

                if (DateTime.UtcNow > expDateTimeUtc)
                {
                    throw new SecurityTokenExpiredException();
                }

                return principal;
            }
            catch (Exception ex)
            {
                throw new JWTExcpetion("Unauthorized - Token is invalid.");
            }
        }

        #endregion




        #region License TOKEN

        public static string CreateJwtForLicense(string name)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes("iiLEzWQM6cORtBVSaemryrNMx0HqB7Gz");

                var identity = new ClaimsIdentity(new Claim[]
                {
                 new Claim(type:"PluginName", name),
                });

                var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = identity,
                    //Expires = DateTime.Now.AddHours(1),
                    //Expires = DateTime.Now.AddHours(1),

                    SigningCredentials = credentials
                };

                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                return jwtTokenHandler.WriteToken(token);
            }
            catch
            {
                throw new JWTExcpetion("Token creation failed!");
            }
        }
        public static ClaimsPrincipal GetPrincipleForLicense(string token)
        {

            try
            {
                var key = Encoding.ASCII.GetBytes("iiLEzWQM6cORtBVSaemryrNMx0HqB7Gz");

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new JWTExcpetion("Unauthorized - Token is invalid.");

                return principal;
            }
            catch (Exception ex)
            {
                throw new JWTExcpetion("Unauthorized - Token is invalid.");
            }
        }

        #endregion
    }
}
