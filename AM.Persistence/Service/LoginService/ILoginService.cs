using System.Collections.Generic;
using System.Threading.Tasks;

using AM.Domain.Entities;


namespace AM.Persistence
{
    public interface ILoginService : IRepositoryService<Login>
    {
        void Update(Login obj);
    }
}
