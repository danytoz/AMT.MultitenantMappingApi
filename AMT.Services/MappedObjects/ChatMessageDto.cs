
namespace AMT.Services.MappedObjects
{
    public class ChatMessageDto
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public string Message { get; set; }
        public string HashedMessage { get; set; }
    }
}
