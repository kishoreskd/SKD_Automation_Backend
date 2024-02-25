using System.Collections.Generic;
using System.Threading.Tasks;

using AM.Domain.Entities;


namespace AM.Persistence
{
    public interface IUserService : IRepositoryService<User>
    {
        void Update(User obj);
    }
}
