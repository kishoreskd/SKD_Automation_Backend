using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using AM.Domain.Entities;


namespace AM.Persistence
{
    public class DepartmentService : RepositoryService<Department>, IDepartmentService
    {
        private readonly AutomationDbService _service;

        public DepartmentService(AutomationDbService service) : base(service)
        {
            this._service = service;
        }

        public void Update(Department obj)
        {
            _service.Department.Update(obj);
        }

      
    }
}
