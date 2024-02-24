using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SKD_Automation
{
    public class ApiError
    {
        public ApiError(int errorCode, string errorMessage, string errorDetails = null, string innerException = null)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
            InnerException = innerException;
        }

        public ApiError()
        {

        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }

        public string InnerException { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
