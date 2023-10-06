
namespace AMT.Services.MappedObjects
{
    public class PasswordDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int HashAlgorithmId { get; set; }
    }
}
