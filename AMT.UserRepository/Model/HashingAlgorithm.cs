using AMT.GenericRepository;

namespace AMT.UserRepository.Model
{
    public class HashingAlgorithm : BaseModel<Guid>
    {
        public string AlgorithmName { get; set; }
        public int Iterations { get; set; }

        public ICollection<Password> Passwords { get; set; }
    }
}
