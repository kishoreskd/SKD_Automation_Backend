using AM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Persistence
{
    public interface IUserModulePrivilegeService : IRepositoryService<UserModulePrivilege>
    {
        void Update(UserModulePrivilege obj);
    }
}
