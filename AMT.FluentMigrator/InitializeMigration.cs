using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;

namespace AMT.FluentMigrator
{
    public class InitializeMigration
    {
        public InitializeMigration(string serverAddress, string databaseName, string userName, string password) 
        {
            string connectionString = $"Server={serverAddress};Database={databaseName};" +
                $"User Id={userName};Password={password};TrustServerCertificate=true;";
            using (var ServiceProvider = CreateServiceProvider(connectionString))
                using(var scope = ServiceProvider.CreateScope())
                {
                    UpdateDatabase(ServiceProvider);
                }
        }

        public ServiceProvider CreateServiceProvider(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(InitializeMigration).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}
