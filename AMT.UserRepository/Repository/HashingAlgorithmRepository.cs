using AMT.GenericRepository.EfCore;
using AMT.UserRepository.Model;
using Microsoft.EntityFrameworkCore;

namespace AMT.UserRepository.Repository
{
    public class HashingAlgorithmRepository: EfRepository<HashingAlgorithm, int>, IHashingAlgorithmRepository
    {
        public HashingAlgorithmRepository(DbContext context) : base(context) { }
    }
}
