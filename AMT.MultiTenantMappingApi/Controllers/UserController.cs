using AMT.DbHelpers.PasswordHelper;
using AMT.Services.MappedObjects;
using AMT.Services.PwdServices;
using AMT.Services.UsrServices;
using AMT.UserRepository.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AMT.MultiTenantMappingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IPasswordServices passwordServices;
        private readonly IUserServices userServices;
        private readonly IMapper mapper;

        public UserController(IUnitOfWorkUser uowUser, IPasswordServices passwordServices, 
            IUserServices userServices, IMapper mapper)
        {
            this.passwordServices = passwordServices;
            this.userServices = userServices;
            this.mapper = mapper;
        }
        // GET Api endpoint
        [HttpPost(Name ="~/GetUserById")]
        public async Task<UserDto> GetUserById(Guid Id)
        {
            var result = await userServices.ValidateUserExistsAsync(Id);
            var userDto = new UserDto();
            if(result.IsFailed)
            {
                userDto.HttpStatusCode = 400;
                userDto.IsSuccess = false;
                userDto.Errors = result.Errors.Select(x=> new AMT.Services.MappedObjects.Response.Error()
                {
                    Message = x.Message
                }).ToList();
                return userDto;
            }
            userDto = result.Value;
            userDto.IsSuccess = true;
            userDto.HttpStatusCode = 200;
            return userDto;
        }

        // POST Api endpoint
        [HttpPost(Name = "~/CreateUser")]
        public async Task<UserDto> Post(CreateUserDtoIn createUserDtoIn)
        {
            var result = await userServices.CreateUserAsync(createUserDtoIn.Username, createUserDtoIn.Name, createUserDtoIn.LastName);
            UserDto userDto = new UserDto();
            if(result.IsFailed)
            {
                userDto.HttpStatusCode = 400;
                userDto.IsSuccess = false;
                userDto.Errors = result.Errors.Select(x=> new AMT.Services.MappedObjects.Response.Error() {
                    Message = x.Message
                }).ToList();
                return userDto;
            }
            userDto = result.Value;
            var passwordResult = await passwordServices.
                CreatePasswordAsync(createUserDtoIn.Username, createUserDtoIn.Password, AlgorithmEnum.BCrypt_9);
            if(passwordResult.IsSuccess)
            {
                userDto.HttpStatusCode = 200;
                userDto.IsSuccess = true;
                return userDto;
            }

            //rollback changes
            var user = await userServices.DeleteAsync(createUserDtoIn.Username);
            userDto.HttpStatusCode = 400;
            userDto.IsSuccess = false;
            userDto.Errors = passwordResult.Errors.Select(x => new AMT.Services.MappedObjects.Response.Error()
            {
                Message = x.Message
            }).ToList();
            return userDto;
        }

        //Put Api endpoint
        [HttpPost(Name = "~/DeleteUser")]
        public async Task<IResult> Delete(string username)
        {
            var result = await userServices.SoftDeleteAsync(username);
            if(result.IsFailed)
            {
                return Results.Ok(result.Errors.Select(x=>x.Message).ToList());
            }
            var userDto = result.Value;
            userDto.HttpStatusCode = 200;
            userDto.IsSuccess = true;
            return Results.Ok(userDto);
        }
    }
}
