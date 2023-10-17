using AMT.Services.MappedObjects.Response;

namespace AMT.Services.MappedObjects
{
    public class PasswordDto: CustomResponseV1
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int HashAlgorithmId { get; set; }
    }

    public class  PasswordDtoIn
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
