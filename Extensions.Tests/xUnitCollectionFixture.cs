using Serilog;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Extensions.Test
{
    public class xUnitCollectionFixture : IDisposable
    {

        public void Dispose()
        {
            // Clean up global objects
        }

        public void ConfigureLogging(ITestOutputHelper output, string ResultsLogFileName)
        {
            // SERILOG Config: https://github.com/serilog/serilog/wiki/Configuration-Basics
            // Write to this log to have output show up as part of the test output.
            // https://github.com/trbenning/serilog-sinks-xunit

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File(
                    new Serilog.Formatting.Json.JsonFormatter(),
                    $"{ResultsLogFileName}.log"
                    //rollingInterval: RollingInterval.Infinite,
                    //restrictedToMinimumLevel: testSettings.LoggerMinimumLevel,
                    //outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}"
                    )
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();
        }
    }

    [CollectionDefinition("xUnitHelper Collection")]
    public class xUnitHelper_Collection : ICollectionFixture<xUnitCollectionFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
