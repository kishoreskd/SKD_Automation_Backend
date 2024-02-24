using System.Collections.Generic;
using System.Threading.Tasks;

using AM.Domain.Entities;


namespace AM.Persistence
{
    public interface IDepartmentService : IRepositoryService<Department>
    {
        void Update(Department obj);
    }
}
