using Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Reflection;

namespace RestApiSample
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // For performance logging
            DateTime dt = DateTime.UtcNow;

            // Get app config info
            IConfiguration config = TttsConfigBuilder();

            // Add the logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            // Write the time for all of the above
            Log.ForContext("SourceContext", "Program.cs").Information("Logger created in {@elapsed} seconds", dt.GetElapsedSeconds());

            // Now we can wrap the entire HostBuilder in try/catch and output any errors.
            try
            {
                dt = DateTime.UtcNow;
                using var host = CreateHostBuilder(args).Build();
                Log.ForContext("SourceContext", "Program.cs").Information("HostBuilder created in {@elapsed} seconds", dt.GetElapsedSeconds());
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.ForContext("SourceContext", "Program.cs").Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IConfiguration TttsConfigBuilder()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly());

            return configBuilder.Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
