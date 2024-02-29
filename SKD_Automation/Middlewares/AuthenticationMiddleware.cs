using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

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
            ApiError err;
            HttpStatusCode sc = HttpStatusCode.Unauthorized;


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

            if (context.Request.Path.StartsWithSegments("/Authenticate/plugin"))
            {
                await _next(context);
                return;
            }

            try
            {

                var header = context.Request.Headers["Authorization"];
                var clientHeader = context.Request.Headers["Auth-Key"];

                if (COM.IsNullOrEmpty(header) && COM.IsNullOrEmpty(clientHeader))
                {
                    err = new ApiError((int)sc, "Unauthorized - Token is missing");
                    context.Response.StatusCode = (int)sc;
                    await context.Response.WriteAsync(err.ToString());
                    return;
                }

                string userToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                string productToken = context.Request.Headers["Auth-Key"].ToString().Replace("Bearer ", "");
                ClaimsPrincipal pricipal = null;

                if (!COM.IsNullOrEmpty(header))
                {
                    pricipal = TokenHelper.GetPrincipalForUserToken(userToken);
                }
                else if (!COM.IsNullOrEmpty(clientHeader))
                {
                    pricipal = TokenHelper.GetPrincipleForProductToken(productToken);
                }
                else
                {
                    err = new ApiError((int)sc, "Unauthorized - Token is missing");
                    context.Response.StatusCode = (int)sc;
                    await context.Response.WriteAsync(err.ToString());
                    return;
                }

                if (COM.IsNull(pricipal))
                {
                    err = new ApiError((int)sc, "Unauthorized - Invalid license token");
                    context.Response.StatusCode = (int)sc;
                    await context.Response.WriteAsync(err.ToString());
                    return;
                };


                context.User = pricipal;
                await _next(context);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                err = new ApiError((int)sc, "Unauthorized - Invalid token");
                context.Response.StatusCode = (int)sc;
                await context.Response.WriteAsync(err.ToString());
                return;
            }
        }
    }
}
