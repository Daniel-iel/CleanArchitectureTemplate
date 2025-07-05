using System.Diagnostics;

namespace RideSharingApp.Infrastructure.Database
{
    public static class MigrateDatabase
    {
        public static void RunFlywayMigration(string connectionString)
        {
            // Caminho para o executável do Flyway (ajuste conforme necessário)
            var flywayPath = "flyway"; // Assumindo que está no PATH
            var migrationDir = Path.Combine(AppContext.BaseDirectory, "..", "RideSharingApp.Infrastructure", "Database", "Migrations");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = flywayPath,
                    Arguments = $"-url=\"{connectionString}\" -locations=filesystem:\"{migrationDir}\" migrate",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Flyway migration failed: {error}");
            }
        }
    }
}
