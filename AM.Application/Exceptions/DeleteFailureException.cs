using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Application.Exceptions
{
    public class DeleteFailureException : Exception
    {
        public DeleteFailureException(string entityName, object key, string message)
             : base($"Deletion of entity \"{entityName}\" {key}) failed, {message}")
        {
        }
    }

}
