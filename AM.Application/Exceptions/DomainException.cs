using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Application.Exceptions
{
    public abstract class DomainException : Exception
    {
        public DomainException(string msg) : base(msg)
        {

        }
    }

    public class JWTExcpetion : DomainException
    {
        public JWTExcpetion(string msg) : base(msg)
        {

        }
    }
}
