using AMT.UserRepository.Model;
using AMT.UserRepository.UnitOfWork;
using FluentResults;
using System.ComponentModel.DataAnnotations;

namespace AMT.Services.UsrServices
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWorkUser uowUser;

        public UserServices(IUnitOfWorkUser uowUser) 
        {
            this.uowUser = uowUser;
        }

        /// <summary>
        /// Function to create a new user
        /// Validate the username
        /// return a result with the user created
        /// </summary>
        /// <param name="username"></param>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public async Task<Result<User>> CreateUserAsync(string username, string name, string lastName)
        {
            var validationResult = await ValidateUserAsync(username);
            if (validationResult.IsFailed)
            {
                return Result.Fail(validationResult.Errors);
            }

            var user = new User()
            {
                Name = name,
                LastName = lastName,
                Username = username,
                CreatedOnUtc = DateTime.UtcNow,
                IsDeleted = false,
                MFAEnabled = false,
                Verified = false

            };

            await uowUser.UserRepository.AddAsync(user);
            await uowUser.UserRepository.SaveChangesAsync();

            return Result.Ok(user);
        }

        /// <summary>
        /// Function to validate the username
        /// Return a result with the validation
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<Result<string>> ValidateUserAsync(string username)
        {
            Result result = new Result();
            if (string.IsNullOrEmpty(username))
            {
                return Result.Fail("Username is empty");
            }
            var emailValidation = new EmailAddressAttribute();
            if (!emailValidation.IsValid(username))
            {
                result.WithError("Username is not a valid email");
            }
            var user = await uowUser.UserRepository.GetFirstOrDefaultAsync(x => x.Username.Equals(username));
            if (user != null)
            {
                result.WithError("Username already exists");
            }
            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            return Result.Ok("Username is valid");
        }
    }
}
