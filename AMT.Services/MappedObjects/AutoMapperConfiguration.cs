using Microsoft.Extensions.DependencyInjection;

namespace AMT.Services.MappedObjects
{
    public class AutoMapperConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ConfigurationProfile));
        }
    }
}
