using AM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Am.Persistence.Seeding
{
    public partial class SeedingSampleData
    {
        public async Task SeedModuleAsync(CancellationToken ct)
        {
            var col = new[]
            {
                new Module{ModuleName=  "Admin"},
                new Module{ModuleName=  "Dashboard"},
                new Module{ModuleName=  "Logging"},
                new Module{ModuleName=  "Access Key"},
            };

            await _service.Module.AddRange(col);
            await _service.Commit();
        }


        public async Task SeedUserPreviledgeModule(CancellationToken ct)
        {
            var col = new[]
            {
                new UserModulePrivilege{
                    UserId= 1,
                    CanCreate = true,
                    CanUpdate=true,
                    CanView=true,
                    CanDelete =true,
                    ModuleId= 1,
                },
                 new UserModulePrivilege{
                    UserId= 1,
                    CanCreate = true,
                    CanUpdate=true,
                    CanView=true,
                    CanDelete =true,
                    ModuleId= 2,
                },
                  new UserModulePrivilege{
                    UserId= 1,
                    CanCreate = true,
                    CanUpdate=true,
                    CanView=true,
                    CanDelete =true,
                    ModuleId= 3,
                },
                   new UserModulePrivilege{
                    UserId= 1,
                    CanCreate = true,
                    CanUpdate=true,
                    CanView=true,
                    CanDelete =true,
                    ModuleId= 4,
                },
                    new UserModulePrivilege{
                    UserId= 2,
                    CanCreate = true,
                    CanUpdate=true,
                    CanView=true,
                    CanDelete =true,
                    ModuleId= 1,
                },
            };

            await _service.UserModulePrivilege.AddRange(col);
            await _service.Commit();
        }
    }
}
