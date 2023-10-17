using AMT.DbHelpers.PasswordHelper;
using AMT.Services.MappedObjects;
using AMT.Services.PwdServices;
using AMT.Services.TokenServices;
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
        private readonly ITokenServices tokenServices;

        public PasswordController(IUnitOfWorkUser uowUser, IPasswordServices passwordServices, 
            ITokenServices tokenServices) {
            this.uowUser = uowUser;
            this.passwordServices = passwordServices;
            this.tokenServices = tokenServices;
        }

        // Post Verify password
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
