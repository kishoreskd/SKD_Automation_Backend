using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.Domain.Entities;


namespace AM.Persistence
{
    public class RoleService : RepositoryService<Role>, IRoleService
    {
        private readonly AutomationDbService _service;


        public RoleService(AutomationDbService context) : base(context)
        {
            this._service = context;
        }

        public void Update(Role obj)
        {
            this._service.Update(obj);
        }
    }
}
