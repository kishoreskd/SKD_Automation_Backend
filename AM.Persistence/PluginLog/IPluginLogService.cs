using System.Collections.Generic;
using System.Threading.Tasks;

using AM.Domain.Entities;


namespace AM.Persistence
{
    public interface IPluginLogService : IRepositoryService<PluginLog>
    {
        void Update(PluginLog obj);

    }
}
