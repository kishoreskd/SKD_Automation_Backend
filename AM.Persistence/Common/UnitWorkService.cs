using RFIM.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using AM.Domain;

namespace AM.Persistence
{
    public class UnitWorkService : IUnitWorkService
    {
        private readonly AutomationDbService _service;

        public UnitWorkService(AutomationDbService service)
        {
            this._service = service;
        }

        public IDepartmentService Department { get { return new DepartmentService(this._service); } }
        public IPluginService Plugin { get { return new PluginService(this._service); } }
        public IPluginLogService PluginLog { get { return new PluginLogService(this._service); } }

        public async Task<int> Commit(CancellationToken cancellationToken = new CancellationToken())
        {
            //foreach (var entry in _service.ChangeTracker.Entries<AuditableEntity>())
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.Entity.CreatedDate = COM.GetUtcToLocal();
            //            break;
            //        case EntityState.Modified:
            //            entry.Entity.LastModifiedDate = COM.GetUtcToLocal();
            //            break;
            //        default:
            //            break;
            //    }
            //}

            return await _service.SaveChangesAsync(cancellationToken);
        }


        // Implement IDisposable pattern for automatic disposal
        private bool disposed = false;
        protected async virtual Task Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //await _contextEmp.DisposeAsync();
                    await _service.DisposeAsync();

                    // Dispose other repositories or resources if needed
                }
                disposed = true;
            }
        }
        public async void Dispose()
        {
            await Dispose(true);

            //By suppressing the finalizer, you're indicating that proper cleanup has already been performed in the Dispose() method. This prevents redundant or duplicate cleanup operations during garbage collection.
            GC.SuppressFinalize(this);
        }
    }
}
