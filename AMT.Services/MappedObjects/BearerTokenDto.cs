using AMT.Services.MappedObjects.Response;

namespace AMT.Services.MappedObjects
{
    public class BearerTokenDto : CustomResponseV1
    {
        public string Token { get; set; }
    }
}
