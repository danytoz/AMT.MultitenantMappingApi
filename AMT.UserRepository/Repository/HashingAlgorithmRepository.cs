using AMT.GenericRepository.EfCore;
using AMT.UserRepository.Model;
using Microsoft.EntityFrameworkCore;

namespace AMT.UserRepository.Repository
{
    public class HashingAlgorithmRepository: EfRepository<HashingAlgorithm, Guid>, IHashingAlgorithmRepository
    {
        public HashingAlgorithmRepository(DbContext context) : base(context) { }
    }
}
