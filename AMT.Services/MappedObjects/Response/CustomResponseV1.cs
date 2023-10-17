
namespace AMT.Services.MappedObjects.Response
{
    public class CustomResponseV1 : ICustomResponseV1
    {
        public int HttpStatusCode { get; set; }
        public List<Error> Errors { get; set; }
        public bool IsSuccess { get; set; }
    }
}
