using AMT.UserRepository.Model;
using AMT.UserRepository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AMT.MultiTenantMappingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatMessageController : ControllerBase
    {
        private readonly IUnitOfWorkUser uowUser;
        public ChatMessageController(IUnitOfWorkUser uowUser)
        {
            this.uowUser = uowUser;
        }

        // GET Api endpoint
        [HttpGet(Name = "GetChatMessagesByUserId")]
        public async Task<IEnumerable<ChatMessage>> Get(Guid userId)
        {
            return await uowUser.ChatMessageRepository.GetManyAsync(x => x.ToUserId == userId);
        }

        [HttpPost(Name = "CreateChatMessage")]
        public async Task<bool> CreateChatMessage(Guid fromUserId, Guid toUserId, string message)
        {
            var fromUser = await uowUser.UserRepository.GetByIdAsync(fromUserId);
            var toUser = await uowUser.UserRepository.GetByIdAsync(toUserId);
            if (fromUser == null || fromUser.IsDeleted.Value)
            {
                throw new Exception("From user not found");
            }
            if (toUser == null || toUser.IsDeleted.Value)
            {
                throw new Exception("To user not found");
            }
            var chatMessage = new ChatMessage
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                Message = message,
                CreatedOnUtc = DateTime.UtcNow,
                IsDeleted = false
            };
            await uowUser.ChatMessageRepository.AddAsync(chatMessage);
            await uowUser.ChatMessageRepository.SaveChangesAsync();
            return true;
        }
    }
}
