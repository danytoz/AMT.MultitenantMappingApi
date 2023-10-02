using AMT.UserRepository.CustomDbContext;
using AMT.UserRepository.Repository;

namespace AMT.UserRepository.UnitOfWork
{
    public class UnitOfWorkUser : IUnitOfWorkUser
    {
        private readonly UserDbContext dbContext;

        public UnitOfWorkUser(UserDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = dbContext;
        }

        private IUserRepository _userRepository;
        public IUserRepository UserRepository => _userRepository ??= new Repository.UserRepository(dbContext);

        private IChatMessageRepository _chatMessageRepository;
        public IChatMessageRepository ChatMessageRepository => _chatMessageRepository ??= new ChatMessageRepository(dbContext);

        private IHashingAlgorithmRepository _hashingAlgorithmRepository;
        public IHashingAlgorithmRepository HashingAlgorithmRepository => _hashingAlgorithmRepository ??= new HashingAlgorithmRepository(dbContext);

        private IPasswordRepository _passwordRepository;
        public IPasswordRepository PasswordRepository => _passwordRepository ??= new PasswordRepository(dbContext);

        public async Task<bool> CompleteAsync() => await dbContext.SaveChangesAsync() > 0;

        public async ValueTask DisposeAsync() => await dbContext.DisposeAsync();
    }
}
