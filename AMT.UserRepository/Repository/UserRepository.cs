using AMT.GenericRepository.EfCore;
using AMT.UserRepository.Model;
using Microsoft.EntityFrameworkCore;

namespace AMT.UserRepository.Repository
{
    public class UserRepository : EfRepository<User, Guid>, IUserRepository
    {
        public DbSet<User> Users { get; set; }

        public UserRepository(DbContext dbContext) : base(dbContext) { }
    }
}
