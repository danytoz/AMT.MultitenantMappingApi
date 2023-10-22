using AMT.Services.MappedObjects;
using FluentResults;

namespace AMT.Services.UsrServices
{
    public interface IUserServices
    {
        Task<Result<string>> ValidateUserAsync(string username);
        Task<Result<UserDto>> CreateUserAsync(string username, string name, string lastName);
        Task<Result<UserDto>> ValidateUserExistsAsync(string username);
        Task<Result<UserDto>> ValidateUserExistsAsync(Guid Id);
        Task<Result<UserDto>> DeleteAsync(string username);
        Task<Result<UserDto>> SoftDeleteAsync(string username);
    }
}
