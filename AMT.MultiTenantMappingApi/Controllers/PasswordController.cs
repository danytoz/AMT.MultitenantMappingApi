using AMT.DbHelpers.PasswordHelper;
using AMT.Services.MappedObjects;
using AMT.Services.PwdServices;
using AMT.Services.TokenServices;
using AMT.Services.UsrServices;
using Microsoft.AspNetCore.Mvc;

namespace AMT.MultiTenantMappingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordServices passwordServices;
        private readonly ITokenServices tokenServices;
        private readonly IUserServices userServices;

        public PasswordController(IPasswordServices passwordServices, 
            ITokenServices tokenServices, IUserServices userServices) {
            this.userServices = userServices;
            this.passwordServices = passwordServices;
            this.tokenServices = tokenServices;
        }

        // Post Verify password
        /// <summary>
        /// Application verifies the username and password and returns a bearer token.
        /// </summary>
        /// <param name="passwordDtoIn"></param>
        /// <returns></returns>
        [HttpPost("~/VerifyPassword")]
        public async Task<BearerTokenDto> VerifyPassword(PasswordDtoIn passwordDtoIn)
        {
            var doesPasswordMatch = await passwordServices.VerifyPasswordAsync(passwordDtoIn.UserName, passwordDtoIn.Password);
            var bearerTokenDto = new BearerTokenDto();
            if(doesPasswordMatch.IsFailed)
            {
                bearerTokenDto.HttpStatusCode = 400;
                bearerTokenDto.IsSuccess = false;
                bearerTokenDto.Errors = doesPasswordMatch.Errors
                    .Select(x => new AMT.Services.MappedObjects.Response.Error() { Message = x.Message }).ToList();
                return bearerTokenDto;
            }
            bearerTokenDto.HttpStatusCode = 200;
            bearerTokenDto.IsSuccess = true;
            bearerTokenDto.Token = tokenServices.GenerateToken(new GenerateTokenProperties()
            {
                UserId = passwordDtoIn.UserId.ToString(),
                Name = passwordDtoIn.UserName,
                Roles = new List<string>() { "ChatUser", "CanCreateRooms"},
            });
            return bearerTokenDto;
        }

        // POST Api endpoint
        [HttpPost(Name ="CreatePassword")]
        public async Task<PasswordDto> Post(string username, string password)
        {
            var user = await userServices.ValidateUserExistsAsync(username);
            if(user.IsFailed)
            {
                var passwordDto = new PasswordDto() {
                    HttpStatusCode = 400,
                    IsSuccess = false,
                    Errors = user.Errors.Select(x => new AMT.Services.MappedObjects.Response.Error() { Message = x.Message }).ToList()
                };
                return passwordDto;
            }
            var userDto  = user.Value;
            if (!userDto.Verified)
            {
                var passwordDto = new PasswordDto()
                {
                    HttpStatusCode = 400,
                    IsSuccess = false,
                    Errors = new List<AMT.Services.MappedObjects.Response.Error>() { new AMT.Services.MappedObjects.Response.Error() { Message = "User not verified" } }
                };
                return passwordDto;
            }

            var passwordEntity = await passwordServices.CreatePasswordAsync(username, password, AlgorithmEnum.BCrypt_9);
            return passwordEntity.Value;
        }
    }
}
