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
        public static string GetConnectionString()
        {
            var appSettingsJson = AppSettingsJson.GetAppSettings();
            return appSettingsJson["ConnectionStrings:EPlastDBConnection"];
        }

    }
}