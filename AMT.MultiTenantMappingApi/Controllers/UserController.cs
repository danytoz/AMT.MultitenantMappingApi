using AMT.DbHelpers.PasswordHelper;
using AMT.Services.MappedObjects;
using AMT.Services.PwdServices;
using AMT.Services.UsrServices;
using AMT.UserRepository.Model;
using AMT.UserRepository.UnitOfWork;
using AutoMapper;
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
        private readonly IMapper mapper;

        public UserController(IUnitOfWorkUser uowUser, IPasswordServices passwordServices, 
            IUserServices userServices, IMapper mapper)
        {
            this.uowUser = uowUser;
            this.passwordServices = passwordServices;
            this.userServices = userServices;
            this.mapper = mapper;
        }
        // GET Api endpoint
        [HttpGet(Name ="GetUserById")]
        public async Task<IResult> Get(Guid Id)
        {
            User user = await uowUser.UserRepository.GetByIdAsync(Id);
            var userDto = mapper.Map<UserDto>(user);
            return Results.Ok(userDto);
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
            var passwordResult = await passwordServices.CreatePasswordAsync(user.Id, password, hashingAlgorithm);
            if(passwordResult.IsSuccess)
            {
                var userDto = mapper.Map<UserDto>(user);
                return Results.Ok(userDto);
            }

            //rollback changes
            uowUser.UserRepository.Delete(user);
            await uowUser.UserRepository.SaveChangesAsync();
            return Results.Ok($"Error creating user {passwordResult.Errors.Select(x=>x.Message)}");
        }
    }
}
