using AMT.DbHelpers.PasswordHelper;
using AMT.Services.PwdServices;
using AMT.Services.UsrServices;
using AMT.UserRepository.Model;
using AMT.UserRepository.UnitOfWork;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace AMT.MultiTenantMappingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWorkUser uowUser;
        private readonly IPasswordServices passwordServices;
        private readonly IUserServices userServices;

        public UserController(IUnitOfWorkUser uowUser, IPasswordServices passwordServices, IUserServices userServices)
        {
            this.uowUser = uowUser;
            this.passwordServices = passwordServices;
            this.userServices = userServices;
        }
        // GET Api endpoint
        [HttpGet(Name ="GetUserById")]
        public async Task<User> Get(Guid Id)
        {
            return await uowUser.UserRepository.GetByIdAsync(Id);
        }

        // POST Api endpoint
        [HttpPost(Name ="CreateUser")]
        public async Task<IResult> Post(string name, string lastName, string username, string password)
        {
            var result = await userServices.CreateUserAsync(username, name, lastName);
            if(result.IsFailed)
            {
                return Results.Ok(result.Errors.Select(x=>x.Message).ToList());
            }
            var user = result.Value;
            var hashingAlgorithm = await uowUser.HashingAlgorithmRepository
                .GetFirstOrDefaultAsync(x => x.AlgorithmName.Equals(AlgorithmEnum.BCrypt_9.ToString()));
            var passwordEntity = passwordServices.CreatePasswordAsync(user.Id, password, hashingAlgorithm);
            return Results.Ok(result.Value);
        }
    }
}
