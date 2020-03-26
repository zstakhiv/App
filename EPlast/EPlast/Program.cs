using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;

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
                .UseStartup<Startup>();
    }
}
