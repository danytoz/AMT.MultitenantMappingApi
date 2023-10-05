using AMT.DbHelpers.PasswordHelper;
using AMT.Services.Validators;
using AMT.UserRepository.Model;
using AMT.UserRepository.UnitOfWork;
using FluentResults;

namespace AMT.Services.PwdServices
{
    public class PasswordServices : IPasswordServices
    {

        private readonly IUnitOfWorkUser uowUser;

        public PasswordServices(IUnitOfWorkUser uowUser)
        {
            this.uowUser = uowUser;
        }

        /// <summary>
        /// Function to validate a password
        /// Using the DefaultPasswordRequirements class
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public Result<bool> ValidatePassword(string password)
        {
            // password validation logic
            var passwordValidator = new DefaultPasswordRequirements();
            var results = new Result();
            if (password.Length < passwordValidator.MinimumLength)
            {
                results.WithError($"Password must be at least {passwordValidator.MinimumLength} characters long");
            }
            if (password.Length > passwordValidator.MaximumLength)
            {
                results.WithError($"Password must be at most {passwordValidator.MaximumLength} characters long");
            }
            if (passwordValidator.NumberOfDigits>0 && !password.Any(char.IsDigit))
            {
                results.WithError("Password must contain at least one digit");
            }
            if (passwordValidator.NumberOfLowerCase>0 && !password.Any(char.IsLower))
            {
                results.WithError("Password must contain at least one lowercase character");
            }
            if (passwordValidator.NumberOfUpperCase>0 && !password.Any(char.IsUpper))
            {
                results.WithError("Password must contain at least one uppercase character");
            }

            if (results.IsFailed)
            {
                return Result.Fail<bool>(results.Errors);
            }
            return Result.Ok(true);
        }

        /// <summary>
        /// Function to create a new password for a user
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="hashingAlgorithm"></param>
        /// <returns></returns>
        public async Task<Result<Password>> CreatePasswordAsync(Guid userId, string password, HashingAlgorithm hashingAlgorithm)
        {
            Password passwordObj = new Password();
            var passwordValidation = ValidatePassword(password);
            if (passwordValidation.IsFailed)
            {
                return Result.Fail<Password>(passwordValidation.Errors);
            }
            var enumValue = Enum.Parse(typeof(AlgorithmEnum), hashingAlgorithm.AlgorithmName);
            switch (enumValue)
            {
                case AlgorithmEnum.BCrypt_9:
                    passwordObj = BCrypt_9(userId, password, hashingAlgorithm);
                    break;
                default:
                    break;
            }

            await uowUser.PasswordRepository.AddAsync(passwordObj);
            await uowUser.PasswordRepository.SaveChangesAsync();
            return Result.Ok(passwordObj);
        }

        /// <summary>
        /// Function to create a new password for a user
        /// Uses BCrypt_9 hashing algorithm
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="hashingAlgorithm"></param>
        /// <returns></returns>
        private Password BCrypt_9(Guid userId, string password, HashingAlgorithm hashingAlgorithm)
        {
            var passwordHashing = new BCryptPasswordHashing(hashingAlgorithm.Iterations);
            var hashedPassword = passwordHashing.HashPassword(password, null);
            var passwordEntity = new Password
            {
                UserId = userId,
                PasswordHash = hashedPassword,
                HashAlgorithmId = hashingAlgorithm.Id,
                CreatedOnUtc = DateTime.UtcNow,
                IsDeleted = false,
                PasswordSalt = "fake salt"
            };
            return passwordEntity;
        }

        /// <summary>
        /// Function to create a new password for a user
        /// Will check if the password is the same as the previous one
        /// Returns the new password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="hashingAlgorithm"></param>
        /// <returns></returns>
        public async Task<Result<Password>> CreateNewPassword(Guid userId, string password, HashingAlgorithm hashingAlgorithm)
        {
            var previousPassword = await uowUser.PasswordRepository.GetFirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted.Value);
            var passwordHashing = new BCryptPasswordHashing(previousPassword.HashAlgorithm.Iterations);
            if(previousPassword == null)
            {
                return await CreateNewPassword(userId, password, hashingAlgorithm);
            }
            if (passwordHashing.VerifyPassword(password, previousPassword.PasswordHash))
            {
                return Result.Fail<Password>("Password is the same as the previous one");
            }
            
            var newPasswordResult = await CreatePasswordAsync(userId, password, hashingAlgorithm);
            if(newPasswordResult.IsFailed)
            {
                return Result.Fail<Password>(newPasswordResult.Errors);
            }
            uowUser.PasswordRepository.SoftDelete(previousPassword);
            await uowUser.PasswordRepository.SaveChangesAsync();
            return Result.Ok(newPasswordResult.Value);
            
        }
    }
}
