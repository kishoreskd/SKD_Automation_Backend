using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Domain.Entities
{
    public class Module : AuditableEntity
    {
        public Module()
        {
            this.UserModulePrivileges = new HashSet<UserModulePrivilege>();
        }

        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public ICollection<UserModulePrivilege> UserModulePrivileges { get; set; }
    }
}
