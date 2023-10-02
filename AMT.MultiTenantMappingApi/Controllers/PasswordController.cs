using AMT.DbHelpers.PasswordHelper;
using AMT.UserRepository.Model;
using AMT.UserRepository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AMT.MultiTenantMappingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IUnitOfWorkUser uowUser;

        public PasswordController(IUnitOfWorkUser uowUser) {
            this.uowUser = uowUser;
        }

        // GET Api endpoint
        [HttpGet(Name ="GetPasswordByUserId")]
        public async Task<bool> Get(Guid id)
        {
            var passwords = await uowUser.PasswordRepository.GetManyAsync(x => x.UserId == id 
            && !x.IsDeleted.Value);
            return passwords.Count() == 1;
        }

        // POST Api endpoint
        [HttpPost(Name ="CreatePassword")]
        public async Task Post(Guid userId, string password)
        {
            var user = await uowUser.UserRepository.GetByIdAsync(userId);
            if(user == null || user.IsDeleted.Value)
            {
                throw new Exception("User not found");
            }
            if (!user.Verified)
            {
                throw new Exception("User not verified");
            }
            var hashingAlgorithm = await uowUser.HashingAlgorithmRepository
                .GetFirstOrDefaultAsync(x=> x.AlgorithmName.Equals(AlgorithmEnum.BCrypt_9));
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
            var currentPassword = await uowUser.PasswordRepository.GetFirstOrDefaultAsync(x=> x.UserId == userId && !x.IsDeleted.Value);
            if(currentPassword != null)
            {
                uowUser.PasswordRepository.SoftDelete(currentPassword);
            }
            await uowUser.PasswordRepository.AddAsync(passwordEntity);
            await uowUser.PasswordRepository.SaveChangesAsync();
        }
    }
}
