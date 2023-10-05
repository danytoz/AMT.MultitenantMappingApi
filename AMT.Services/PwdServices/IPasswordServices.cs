using AMT.UserRepository.Model;
using FluentResults;

namespace AMT.Services.PwdServices
{
    public interface IPasswordServices
    {
        Task<Result<Password>> CreateNewPassword(Guid userId, string password, HashingAlgorithm hashingAlgorithm);
        Task<Result<Password>> CreatePasswordAsync(Guid userId, string password, HashingAlgorithm hashingAlgorithm);
        Result<bool> ValidatePassword(string password);
    }
}
