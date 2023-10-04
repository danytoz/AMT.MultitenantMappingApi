using AMT.GenericRepository;

namespace AMT.UserRepository.Model
{
    public class Password : BaseModel<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int HashAlgorithmId { get; set; }

        public virtual HashingAlgorithm HashAlgorithm { get; set; }
        public virtual User User { get; set; }
    }
}
