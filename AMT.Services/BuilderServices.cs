using AMT.Services.PwdServices;
using AMT.Services.TokenServices;
using AMT.Services.UsrServices;
using Microsoft.Extensions.DependencyInjection;

namespace AMT.Services
{
    public static class BuilderServicesServices
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordServices, PasswordServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ITokenServices, BearerTokenServices>();
        }
    }
}
