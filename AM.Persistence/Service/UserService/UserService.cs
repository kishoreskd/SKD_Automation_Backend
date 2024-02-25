using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AM.Domain.Entities;


namespace AM.Persistence
{
    public class UserService : RepositoryService<User>, IUserService
    {
        private readonly AutomationDbService _service;

        public UserService(AutomationDbService context) : base(context)
        {
            this._service = context;
        }

        public void Update(User obj)
        {
            _service.User.Update(obj);
        }
    }
}
