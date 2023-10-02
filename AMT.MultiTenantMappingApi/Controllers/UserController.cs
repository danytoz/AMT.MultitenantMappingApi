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

        public UserController(IUnitOfWorkUser uowUser) {
            this.uowUser = uowUser;
        }
        // GET Api endpoint
        [HttpGet(Name ="GetUserById")]
        public async Task<User> Get(Guid Id)
        {
            return await uowUser.UserRepository.GetByIdAsync(Id);
        }
    }
}
