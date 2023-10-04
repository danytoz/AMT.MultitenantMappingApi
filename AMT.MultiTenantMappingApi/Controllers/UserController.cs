using AMT.DbHelpers.PasswordHelper;
using AMT.Services.PwdServices;
using AMT.UserRepository.Model;
using AMT.UserRepository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace AMT.MultiTenantMappingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWorkUser uowUser;
        private readonly IPasswordServices passwordServices;

        public UserController(IUnitOfWorkUser uowUser, IPasswordServices passwordServices = null)
        {
            this.uowUser = uowUser;
            this.passwordServices = passwordServices;
        }
        // GET Api endpoint
        [HttpGet(Name ="GetUserById")]
        public async Task<User> Get(Guid Id)
        {
            return await uowUser.UserRepository.GetByIdAsync(Id);
        }

        // POST Api endpoint
        [HttpPost(Name ="CreateUser")]
        public async Task Post(string name, string lastName, string username, string password)
        {
            var user = new User()
            {
                Name = name,
                LastName = lastName,
                Username = username,
                CreatedOnUtc = DateTime.UtcNow,
                IsDeleted = false,
                MFAEnabled= false,
                Verified = false

            };
            await uowUser.UserRepository.AddAsync(user);
            await uowUser.UserRepository.SaveChangesAsync();
            var hashingAlgorithm = await uowUser.HashingAlgorithmRepository
                .GetFirstOrDefaultAsync(x => x.AlgorithmName.Equals(AlgorithmEnum.BCrypt_9.ToString()));
            var passwordEntity = passwordServices.CreatePassword(user.Id, password, hashingAlgorithm);

            await uowUser.PasswordRepository.AddAsync(passwordEntity);
            await uowUser.PasswordRepository.SaveChangesAsync();
        }
    }
}
