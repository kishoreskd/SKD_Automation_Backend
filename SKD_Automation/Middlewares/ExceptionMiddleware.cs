using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                ApiError.ApiError response;
                HttpStatusCode sc = HttpStatusCode.InternalServerError;

                if (_env.IsDevelopment())
                {
                    response = new ApiError.ApiError((int)sc, ex.Message, ex.StackTrace.ToString(), ex.InnerException.ToString());
                }
                else
                {
                    response = new ApiError.ApiError((int)sc, ex.Message);
                }

                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)sc;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response.ToString());
            }
        }
    }
}
