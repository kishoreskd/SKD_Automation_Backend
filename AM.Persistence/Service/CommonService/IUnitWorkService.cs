using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM.Domain.Entities;


namespace AM.Persistence
{
    public interface IUnitWorkService : IDisposable
    {
        IDepartmentService Department { get; }
        IPluginService Plugin { get; }
        IPluginLogService PluginLog { get; }
        ILoginService Login { get; }
        IRoleService Role { get; }

        Task<int> Commit(CancellationToken cancellationToken = new CancellationToken());
    }
}
