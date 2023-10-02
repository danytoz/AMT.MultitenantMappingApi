using AMT.GenericRepository.EfCore;
using AMT.UserRepository.Model;
using Microsoft.EntityFrameworkCore;

namespace AMT.UserRepository.Repository
{
    public class ChatMessageRepository :EfRepository<ChatMessage, int>, IChatMessageRepository
    {
        public ChatMessageRepository(DbContext context) : base(context) { }
    }
}
