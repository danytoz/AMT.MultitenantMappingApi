using AMT.DbHelpers.PasswordHelper;
using AMT.Services.MappedObjects;
using FluentResults;

namespace AMT.Services.PwdServices
{
    public interface IPasswordServices
    {
        Task<Result<PasswordDto>> CreateNewPasswordAsync(string username, string password, AlgorithmEnum algorithm);
        Task<Result<PasswordDto>> CreatePasswordAsync(string username, string password, AlgorithmEnum algorithm);
        Result<bool> ValidatePassword(string password);
        Task<Result<bool>> VerifyPasswordAsync(Guid userId, string password);
        Task<Result<bool>> VerifyPasswordAsync(string userName, string password);
    }
}
