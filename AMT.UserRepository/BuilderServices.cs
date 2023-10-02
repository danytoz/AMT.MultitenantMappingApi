using Microsoft.Extensions.DependencyInjection;
using AMT.UserRepository.CustomDbContext;
using Microsoft.EntityFrameworkCore;

namespace AMT.UserRepository
{
    public static class BuilderServices
    {
        public static void AddUserDbContext(this IServiceCollection services, Action<DbContextModel> action)
        {
            DbContextModel dbContextModel = new DbContextModel();
            action(dbContextModel);
            var sqlConnection = $"Server={dbContextModel.ServerAddress};Database={dbContextModel.DatabaseName};User Id={dbContextModel.UserName};Password={dbContextModel.Password};";
            if(dbContextModel.TrustServerCertificate)
            {
                sqlConnection += "TrustServerCertificate=true;";
            }
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseSqlServer(sqlConnection);
            });
        }
    }
}
