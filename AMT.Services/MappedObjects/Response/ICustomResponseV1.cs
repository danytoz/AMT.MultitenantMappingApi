
namespace AMT.Services.MappedObjects.Response
{
    public interface ICustomResponseV1
    {
        int HttpStatusCode { get; set; }
        List<Error> Errors { get; set; }
        bool IsSuccess { get; set; }
    }
}
