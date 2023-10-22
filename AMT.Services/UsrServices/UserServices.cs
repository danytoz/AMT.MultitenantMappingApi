using AMT.Services.MappedObjects;
using AMT.UserRepository.Model;
using AMT.UserRepository.UnitOfWork;
using AutoMapper;
using FluentResults;
using System.ComponentModel.DataAnnotations;

namespace AMT.Services.UsrServices
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWorkUser uowUser;
        private readonly IMapper mapper;

        public UserServices(IUnitOfWorkUser uowUser, IMapper mapper) 
        {
            this.uowUser = uowUser;
            this.mapper = mapper;
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
        public async Task<Result<UserDto>> CreateUserAsync(string username, string name, string lastName)
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

            return Result.Ok(mapper.Map<UserDto>(user));
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

        public async Task<Result<UserDto>> ValidateUserExistsAsync(string username)
        {
            var user = await uowUser.UserRepository.
                GetFirstOrDefaultAsync(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return Result.Fail<UserDto>("User not found");
            }
            return Result.Ok(mapper.Map<UserDto>(user));
        }

        public async Task<Result<UserDto>> ValidateUserExistsAsync(Guid Id)
        {
            var user = await uowUser.UserRepository.GetByIdAsync(Id);
            if (user == null)
            {
                return Result.Fail<UserDto>("User not found");
            }
            return Result.Ok(mapper.Map<UserDto>(user));
        }

        public async Task<Result<UserDto>> DeleteAsync(string username)
        {
            var user = await uowUser.UserRepository.
                GetFirstOrDefaultAsync(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return Result.Fail<UserDto>("User not found");
            }
            uowUser.UserRepository.Delete(user);
            await uowUser.UserRepository.SaveChangesAsync();
            return Result.Ok(mapper.Map<UserDto>(user));
        }

        public async Task<Result<UserDto>> SoftDeleteAsync(string username)
        {
            var user = await uowUser.UserRepository.
                GetFirstOrDefaultAsync(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return Result.Fail<UserDto>("User not found");
            }
            user.IsDeleted = true;
            uowUser.UserRepository.Update(user);
            await uowUser.UserRepository.SaveChangesAsync();
            return Result.Ok(mapper.Map<UserDto>(user));
        }
    }
}
