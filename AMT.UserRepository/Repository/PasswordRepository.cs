using AMT.GenericRepository.EfCore;
using AMT.UserRepository.Model;
using Microsoft.EntityFrameworkCore;

namespace AMT.UserRepository.Repository
{
    public class PasswordRepository : EfRepository<Password, Guid>, IPasswordRepository
    {
        public PasswordRepository(DbContext context) : base(context) { }
    }
}
