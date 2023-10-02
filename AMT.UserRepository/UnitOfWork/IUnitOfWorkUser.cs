
using AMT.UserRepository.Repository;

namespace AMT.UserRepository.UnitOfWork
{
    public interface IUnitOfWorkUser :IAsyncDisposable
    {
        IChatMessageRepository ChatMessageRepository { get; }
        IHashingAlgorithmRepository HashingAlgorithmRepository { get; }
        IUserRepository UserRepository { get; }
        IPasswordRepository PasswordRepository { get; }

        Task<bool> CompleteAsync();
    }
}
