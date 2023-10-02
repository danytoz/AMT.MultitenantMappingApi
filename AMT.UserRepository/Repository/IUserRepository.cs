using AMT.GenericRepository;
using AMT.UserRepository.Model;

namespace AMT.UserRepository.Repository
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
