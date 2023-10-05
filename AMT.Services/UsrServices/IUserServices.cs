
using AMT.UserRepository.Model;
using FluentResults;

namespace AMT.Services.UsrServices
{
    public interface IUserServices
    {
        Task<Result<string>> ValidateUserAsync(string username);
        Task<Result<User>> CreateUserAsync(string username, string name, string lastName);
    }
}
