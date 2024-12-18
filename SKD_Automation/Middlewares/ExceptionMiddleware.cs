﻿using AM.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SKD_Automation.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace SKD_Automation.Middlewares
{
    //Helps to catch the exception at global level for every request.

    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            ApiError response;
            HttpStatusCode sc = HttpStatusCode.InternalServerError;

            try
            {
                await _next(context);
            }
            catch (JWTExcpetion ex)
            {
                string msg = ex.Message;
                sc = HttpStatusCode.Unauthorized;
                response = new ApiError((int)sc, ex.Message);
                context.Response.StatusCode = (int)sc;
                await context.Response.WriteAsync(response.ToString());
                return;
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    response = new ApiError((int)sc, ex.Message, ex.StackTrace.ToString(), ex.InnerException?.ToString());
                }
                else
                {
                    response = new ApiError((int)sc, ex.Message);
                }

                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)sc;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response.ToString());
            }
        }
    }
}
