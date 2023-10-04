using AMT.DbHelpers.PasswordHelper;
using AMT.UserRepository.Model;

namespace AMT.Services.PwdServices
{
    public class PasswordServices : IPasswordServices
    {
        public Password CreatePassword(Guid userId, string password, HashingAlgorithm hashingAlgorithm)
        {
            Password passwordObj = new Password();
            var enumValue = Enum.Parse(typeof(AlgorithmEnum), hashingAlgorithm.AlgorithmName);
            switch (enumValue)
            {
                case AlgorithmEnum.BCrypt_9:
                    passwordObj = BCrypt_9(userId, password, hashingAlgorithm);
                    break;
                default:
                    break;
            }
            return passwordObj;
        }

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
    }
}
