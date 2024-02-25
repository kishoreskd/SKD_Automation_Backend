using AM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Dto
{
    public class LoginDto
    {      
        public string UserName { get; set; }
        public string Password { get; set; }  
    }
}
