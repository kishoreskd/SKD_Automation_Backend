using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AM.Domain.Entities;


namespace AM.Persistence
{
    public interface IPluginService : IRepositoryService<Plugin>
    {
        void Update(Plugin obj);
        void DetachLocal(List<Plugin> obj);
        //IQueryable<Plugin> GetAllPluginWithChild();

    }
}
