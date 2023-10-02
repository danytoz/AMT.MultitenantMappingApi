using AMT.GenericRepository;
using AMT.UserRepository.Model;

namespace AMT.UserRepository.Repository
{
    public interface IPasswordRepository: IRepository<Password, Guid>
    {
    }
}
