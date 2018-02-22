using DbUp;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace MicroserviceTemplate.Service.DBMigration
{
    class Program
    {
        const string DBConnectionStringName = "MicroserviceTemplateDbContext";
        static int Main(string[] args)
        {
            var connectionString =
                args.FirstOrDefault()
                ?? ConfigurationManager.ConnectionStrings[DBConnectionStringName].ConnectionString;

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();

                #if DEBUG
                Console.ReadLine();
                #endif

                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

            Console.ReadLine();
            return 0;
        }
    }
}
