﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace SKD_Automation.Filters
{
    public class HeaderAuthorizationFilterForLicense : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            HttpStatusCode sc = HttpStatusCode.Unauthorized;

            var header = context.HttpContext.Request.Headers[SD.HeadersKey.License];

            if (COM.IsNullOrEmpty(header))
            {
                var error = new ApiError()
                {
                    ErrorCode = (int)sc,
                    ErrorMessage = "Unauthorized"
                };

                var response = new ContentResult()
                {
                    StatusCode = 401,
                    Content = error.ToString(),
                    ContentType = "application/json",
                };

                context.Result = response;
            }
        }
    }
}