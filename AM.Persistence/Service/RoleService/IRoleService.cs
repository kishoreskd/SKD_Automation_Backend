using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using AM.Domain.Entities;

namespace AM.Persistence
{
    public interface IRoleService : IRepositoryService<Role>
    {
        void Update(Role obj);
    }
}
