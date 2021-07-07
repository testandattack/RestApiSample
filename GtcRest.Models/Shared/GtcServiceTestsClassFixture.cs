using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace GtcRest.Models.Shared
{
    public class GtcServiceTestsClassFixture : IDisposable
    {
        public Settings settings { get; set; }

        public IOptionsSnapshot<Settings> snapshotSettings { get; private set; }

        public GtcServiceTestsClassFixture()
        {
            // Read the core settings from appsettings, environment, secrets
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();
            IConfigurationRoot rootSettings = configBuilder.Build();

            // Bind the appSettings to the config
            settings = new Settings();
            rootSettings.Bind(settings);

            snapshotSettings = Settings.CreateIOptionSnapshotMock(settings);
        }

        // This takes the place of the logger init in the main app's program.cs file. We need
        // it here since we do not actually run the entire program when we run unit tests.
        public void ConfigureLogging(ITestOutputHelper output)
        {
            // SERILOG Config: https://github.com/serilog/serilog/wiki/Configuration-Basics
            // Write to this log to have output show up as part of the test output.
            // https://github.com/trbenning/serilog-sinks-xunit

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();
        }

        public void Dispose()
        {
            // Clean up global objects
        }
    }
}
