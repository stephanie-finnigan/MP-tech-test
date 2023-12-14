using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace Moonpig.PostOffice.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.ConfigureLogging((hostingContext, builder) =>
                    {
                        builder
                        .AddApplicationInsights()
                        .AddFilter<ApplicationInsightsLoggerProvider>("SimpleLogger", LogLevel.Debug)
                        .SetMinimumLevel(LogLevel.Warning);
                    });
                });
    }
}
