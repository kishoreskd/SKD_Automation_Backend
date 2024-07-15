using AM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM.Persistence
{
    public interface IModuleService : IRepositoryService<Module>
    {
        void Update(Module obj);
    }
}
