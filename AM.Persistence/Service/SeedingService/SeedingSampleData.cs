using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM.Domain.Entities;
using AM.Persistence;

namespace Am.Persistence.Seeding
{
    public class SeedingSampleData
    {
        public IUnitWorkService _service;

        public SeedingSampleData(IUnitWorkService service)
        {
            this._service = service;
        }

        public async Task SeedAllAsync(CancellationToken cncellationToken = default)
        {
            //if (_service.Plugin.IsAny())
            //{
            //    return;
            //}

            await this.SeedRoleAsync(cncellationToken);

            //await SeedDepartmentAsync(cncellationToken);
            //await SeedPluginAsync(cncellationToken);
            //await SeedPluginLogAsync(cncellationToken);
        }
        public async Task SeedDepartmentAsync(CancellationToken cncellationToken)
        {
            var departmentCol = new[]
            {
                new Department {  DepartmentName = "STEEL" },
                new Department {  DepartmentName = "CONCRETE" },
                new Department {  DepartmentName = "SDS2" },
                new Department {  DepartmentName = "AUTOCAD" },
                new Department {  DepartmentName = "REVIT" }
            };

            await _service.Department.AddRange(departmentCol);
            await _service.Commit();
        }
        public async Task SeedPluginAsync(CancellationToken cncellationToken)
        {
            var desCol = new[]
            {
                new Plugin {  PluginName = "Plugin1", ManualMinutes = 30, AutomatedMinutes = 1, DepartmentId = 1, Description = "-", CreatedBy = 2701, CreatedDate = COM.GetUtcToLocal(),
                LastModifiedBy = null, LastModifiedDate = null},
                 new Plugin {  PluginName = "Plugin2", ManualMinutes = 30, AutomatedMinutes = 2, DepartmentId = 1, Description = "-", CreatedBy = 2701, CreatedDate = COM.GetUtcToLocal(),
                LastModifiedBy = null, LastModifiedDate = null},
                new Plugin {  PluginName = "Plugin3", ManualMinutes = 20, AutomatedMinutes = 1, DepartmentId = 1, Description = "-", CreatedBy = 2701, CreatedDate = COM.GetUtcToLocal(),
                LastModifiedBy = null, LastModifiedDate = null},
                new Plugin {  PluginName = "Plugin4", ManualMinutes = 20, AutomatedMinutes = 5, DepartmentId = 1, Description = "-", CreatedBy = 2701, CreatedDate = COM.GetUtcToLocal(),
                LastModifiedBy = null, LastModifiedDate = null},
                new Plugin {  PluginName = "Plugin5", ManualMinutes = 10, AutomatedMinutes = 8, DepartmentId = 1, Description = "-", CreatedBy = 2701, CreatedDate = COM.GetUtcToLocal(),
                LastModifiedBy = null, LastModifiedDate = null},

            };

            await _service.Plugin.AddRange(desCol);
            await _service.Commit();
        }
        public async Task SeedPluginLogAsync(CancellationToken cncellationToken)
        {
            var locCol = new[]
            {
                new PluginLog {  JobName = "Job1", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 1  },
                new PluginLog {  JobName = "Job2", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 1  },
                new PluginLog {  JobName = "Job3", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 2  },
                new PluginLog {  JobName = "Job4", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 1  },
                new PluginLog {  JobName = "Job5", CreatedBy = 2704,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 3  },
                new PluginLog {  JobName = "Job6", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 1  },
                new PluginLog {  JobName = "Job7", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 4  },
                new PluginLog {  JobName = "Job8", CreatedBy = 2706,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 1  },
                new PluginLog {  JobName = "Job9", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 1  },
                new PluginLog {  JobName = "Job10", CreatedBy = 2708,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId =5  },
                new PluginLog {  JobName = "Job11", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 3  },
                new PluginLog {  JobName = "Job12", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 2  },
                new PluginLog {  JobName = "Job10", CreatedBy = 2709,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 1  },
                new PluginLog {  JobName = "Job10", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 5  },
                new PluginLog {  JobName = "Job10", CreatedBy = 2703,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 2  },
                new PluginLog {  JobName = "Job10", CreatedBy = 2701,CreatedDate = COM.GetUtcToLocal(),Activity = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",LastModifiedBy = null, LastModifiedDate = null, PluginId = 1  },
            };

            await _service.PluginLog.AddRange(locCol);
            await _service.Commit();
        }

        public async Task SeedRoleAsync(CancellationToken cancellationToke)
        {
            var col = new[]
            {
                new Role {RoleName = "ADMIN"},
                new Role {RoleName= "MANAGER"},
                new Role {RoleName = "TEAMLEAD"},
                new Role {RoleName = "USER"}
            };
        }
    }
}
