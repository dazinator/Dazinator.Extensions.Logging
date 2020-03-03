using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Hosting;
using System;
using System.Linq;
using Xunit;

namespace Dazinator.Extensions.Logging.Tests
{
    public partial class SerilogTests
    {

        [Fact]
        public void Can_Adjust_LogLevel()
        {

            var testLogSink = new TestSink();
            var assertProvider = new TestLoggerProvider(testLogSink);

            var serilogContext = SerilogLoggerProviderFactory.CreateLogger((loggerConfig) =>
            {
                loggerConfig.Enrich.FromLogContext()
                            .WriteTo.Provider(assertProvider);
            });

            serilogContext.LogLevelSwitch.MinimumLevel = LogLevel.Information;

            var services = new ServiceCollection();
            services.AddLogging(b =>
            {
                serilogContext.Register(b);
            });

            var newSp = services.BuildServiceProvider();
            var logger = newSp.GetRequiredService<ILogger<SwitchLogLevelLoggerTests>>();

            // Assert
            logger.LogDebug("This is a DEBUG message you won't see this because switch set by default to LogLevel.Information");
            var writes = testLogSink.Writes;
            Assert.Empty(writes);

            // Could use serilogContext.LogLevelSwitch directly, but just proving the switch is also availabel for DI.
            var adjustableSwitch = newSp.GetRequiredService<ILoggingLevelSwitch>();
            adjustableSwitch.MinimumLevel = LogLevel.Debug;

            logger.LogDebug("You should see this now!");
            Assert.Single(writes);
        }

        [Fact]
        public void Can_Use_DiagnosticContext()
        {

            var testLogSink = new TestSink();
            var assertProvider = new TestLoggerProvider(testLogSink);
            var serilogContext = SerilogLoggerProviderFactory.CreateLogger((loggerConfig) =>
            {
                loggerConfig.Enrich.FromLogContext()
                            .WriteTo.Provider(assertProvider);
            }, providerOwnsLogger: false, addDiagnosticContext: true);

            serilogContext.LogLevelSwitch.MinimumLevel = LogLevel.Information;

            var services = new ServiceCollection();
            services.AddLogging(b =>
            {
                serilogContext.Register(b);
            });

            var newSp = services.BuildServiceProvider();

            var serilogDiagnosticContext = newSp.GetRequiredService<DiagnosticContext>();
            var collector = serilogDiagnosticContext.BeginCollection();

            var diagnosticContext = newSp.GetRequiredService<IRequestDiagnosticLogContext>();
            diagnosticContext.SetProperty("FOO", "BAR");

            if (!collector.TryComplete(out var props))
            {
                throw new Exception();
            }
            props.Single(a => a.Name == "FOO");



            // Assert
            var writes = testLogSink.Writes;
          //  Assert.Empty(writes);          
        }
    }
}
