using AM.Domain.Entities;

namespace AM.Persistence
{
    public class UserModulePrivilegeService : RepositoryService<UserModulePrivilege>, IUserModulePrivilegeService
    {
        private readonly AutomationDbService _service;

        public UserModulePrivilegeService(AutomationDbService context) : base(context)
        {
            this._service = context;
        }

        public void Update(UserModulePrivilege obj)
        {
            _service.UserModulePrivilege.Update(obj);
        }
    }
}
