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
        IUserService User { get; }
        IRoleService Role { get; }
        IModuleService Module { get; }
        IUserModulePrivilegeService UserModulePrivilege { get; }

        Task<int> Commit(CancellationToken cancellationToken = new CancellationToken());
    }
}
