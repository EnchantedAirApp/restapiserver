using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOfTricks.ModernSatyrMedia.Extensions

{
    public static class DBExtenstions
    {
        public static void EnsureDatabaseCreated<T>(this IServiceProvider services) where T : DbContext
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<T>();

                // Ensure the database is created before applying migrations
                if (dbContext.Database.EnsureCreated())
                {
                    // The database did not exist and was created
                    Console.WriteLine("Database created for the first time.");
                }
                else
                {
                    // The database already exists, apply migrations
                    dbContext.Database.Migrate();
                    Console.WriteLine("Migrations applied.");
                }
            }
        }
        public static void SetMySQLSettings(this DbContextOptionsBuilder options)
        {
            // Use environment variables to determine the database connection string or default to a local configuration
            var connectionString = "server=localhost;port=3306;database=enchantedair;user=root;password=212FuckYou!"; //Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            options.UseMySql(
            connectionString,
                ServerVersion.AutoDetect(connectionString), // Automatically detect the MySQL server version
            mysqlOptions =>
            {
                mysqlOptions.MigrationsAssembly(typeof(DBExtenstions).Assembly.FullName); // Ensure migrations are generated in the current assembly
            });
        }
        public static void SetSQLiteSettings(this DbContextOptionsBuilder options)
        {
            // Use environment variables to determine the database path, or default to a local file
            var databasePath = Environment.GetEnvironmentVariable("DB_PATH") ?? "Data Source=AppDB.sqlite";

            options.UseSqlite(
                databasePath,
                sqliteOptions =>
                {
                    sqliteOptions.MigrationsAssembly(typeof(DBExtenstions).Assembly.FullName); // Ensure migrations are generated in the current assembly
                });
        }

    }
}