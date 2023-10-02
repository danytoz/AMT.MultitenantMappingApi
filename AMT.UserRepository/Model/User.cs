using AMT.GenericRepository;

namespace AMT.UserRepository.Model
{
    public class User : BaseModel<Guid>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool MFAEnabled { get; set; }
        public bool Verified { get; set; }

        public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; }
        public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; }
        public virtual ICollection<Password> Passwords { get; set; }
    }
}