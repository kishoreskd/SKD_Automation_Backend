using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
