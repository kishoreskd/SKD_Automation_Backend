using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AM.Domain.Entities;


namespace AM.Persistence
{
    public class LoginService : RepositoryService<Login>, ILoginService
    {
        private readonly AutomationDbService _service;

        public LoginService(AutomationDbService context) : base(context)
        {
            this._service = context;
        }

        public void Update(Login obj)
        {
            _service.Login.Update(obj);
        }

       

    }
}
