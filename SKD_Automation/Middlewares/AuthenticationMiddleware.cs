using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AM.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using SKD_Automation.Helper;


namespace SKD_Automation.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/Login/authenticate"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments("/Login/token/refresh"))
            {
                await _next(context);
                return;
            }
            
            var header = context.Request.Headers[SD.HeadersKey.Login];
            var clientHeader = context.Request.Headers[SD.HeadersKey.License];

            if ((COM.IsNullOrEmpty(header) && COM.IsNullOrEmpty(clientHeader)))
            {
                throw new JWTExcpetion("Unauthorized - Token is missing.");
            }

            ClaimsPrincipal pricipal = null;

            if (!COM.IsNullOrEmpty(header))
            {
                string loginToken = context.Request.Headers[SD.HeadersKey.Login].ToString().Replace("Bearer ", "");
                pricipal = TokenHelper.GetPrincipalForLgn(loginToken);
            }
            else if (!COM.IsNullOrEmpty(clientHeader))
            {
                string licenseToken = context.Request.Headers[SD.HeadersKey.License].ToString().Replace("Bearer ", "");
                pricipal = TokenHelper.GetPrincipleForLicense(licenseToken);
            }
            else
            {
                throw new JWTExcpetion("Unauthorized - Token is missing.");
            }

            if (COM.IsNull(pricipal))
            {
                throw new JWTExcpetion("Unauthorized - Invalid license token.");
            };

            context.User = pricipal;
            await _next(context);
        }
    }
}
