using AMT.Services.MappedObjects.Response;

namespace AMT.Services.MappedObjects
{
    public class ChatMessageDto : CustomResponseV1
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public string Message { get; set; }
        public string HashedMessage { get; set; }
    }
}
