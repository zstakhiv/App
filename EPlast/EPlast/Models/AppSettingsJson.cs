using EPlast.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EPlast.Controllers
{
    public static class AppSettingsJson
    {
        private static string ApplicationExeDirectory()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            return appRoot;
        }

        private static IConfigurationRoot GetAppSettings()
        {
            string applicationExeDirectory = ApplicationExeDirectory();

            var builder = new ConfigurationBuilder()
            .SetBasePath(applicationExeDirectory)
            .AddJsonFile("appsettings.json");

            return builder.Build();
        }

        public static DbContextOptions<EPlastDBContext> GetConnectionString()
        {
            var appSettingsJson = GetAppSettings();
            var connectionString = GetConnectionString();
            var options = new DbContextOptionsBuilder<EPlastDBContext>();
            options.UseSqlServer(appSettingsJson["ConnectionStrings:EPlastDBConnection"]);
            return options.Options;
        }
    }
}