using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace EPlast
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddNLog();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                if (context.HostingEnvironment.IsProduction())
                {
                    var builtConfig = config.Build();

                    using (var store = new X509Store(StoreLocation.CurrentUser))
                    {
                        store.Open(OpenFlags.ReadOnly);
                        var certs = store.Certificates
                            .Find(X509FindType.FindByThumbprint,
                                builtConfig["AzureADCertThumbprint"], false);

                        config.AddAzureKeyVault(
                            $"https://{builtConfig["ep-kv-dev"]}.vault.azure.net/",
                            builtConfig["AzureADApplicationId"],
                            certs.OfType<X509Certificate2>().Single());

                        store.Close();
                    }
                }
            })
                .UseStartup<Startup>();

    }
}
