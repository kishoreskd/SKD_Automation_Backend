using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SKD_Automation.Filters
{
    //Second execute
    public class SampleActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine("Action filter after");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.WriteLine("Action filter before");

        }
    }

    //First Execute
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Debug.WriteLine("AuthFilterCalled");
        }
    }

    public class LoginFilterAsyncAttribute : Attribute, IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
