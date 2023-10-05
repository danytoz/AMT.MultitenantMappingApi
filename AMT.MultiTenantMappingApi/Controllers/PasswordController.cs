using AMT.DbHelpers.PasswordHelper;
using AMT.Services.PwdServices;
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
        private readonly IPasswordServices passwordServices;

        public PasswordController(IUnitOfWorkUser uowUser, IPasswordServices passwordServices) {
            this.uowUser = uowUser;
            this.passwordServices = passwordServices;
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
            var passwordEntity = await passwordServices.CreatePasswordAsync(userId, password, hashingAlgorithm);
            Results.Ok();
        }
    }
}
