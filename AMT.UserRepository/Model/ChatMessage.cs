using AMT.GenericRepository;

namespace AMT.UserRepository.Model
{
    public class ChatMessage : BaseModel<int>
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public string Message { get; set; }
        public string HashedMessage { get; set; }
        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
