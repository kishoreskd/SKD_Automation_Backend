using AM.Domain.Entities;

namespace AM.Persistence
{
    public class ModuleService : RepositoryService<Module>, IModuleService
    {
        private readonly AutomationDbService _service;

        public ModuleService(AutomationDbService context) : base(context)
        {
            this._service = context;
        }

        public void Update(Module obj)
        {
            _service.Module.Update(obj);
        }
    }
}
