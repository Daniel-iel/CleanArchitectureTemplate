using DbUp;
using Microsoft.Extensions.Configuration;

namespace RideSharingApp.Infrastructure.Migrations
{
    public static class DbUpMigrator
    {
        public static void RunMigration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RideConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("String de conexão não encontrada. Migration não executada.");
                return;
            }

            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsFromFileSystem($"{AppContext.BaseDirectory}/Migrations")
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                throw result.Error;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Migration executada com sucesso!");
                Console.ResetColor();
            }
        }
    }
}
