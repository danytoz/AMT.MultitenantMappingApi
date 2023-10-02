using AMT.GenericRepository;
using AMT.UserRepository.Model;

namespace AMT.UserRepository.Repository
{
    public interface IChatMessageRepository : IRepository<ChatMessage, int>
    {
    }
}
