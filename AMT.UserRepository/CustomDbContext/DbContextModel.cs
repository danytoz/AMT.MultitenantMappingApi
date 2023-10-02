
namespace AMT.UserRepository.CustomDbContext
{
    public class DbContextModel
    {
        public string ServerAddress { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool TrustServerCertificate { get; set; }
    }
}
