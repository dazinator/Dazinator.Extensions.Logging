using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Xunit;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Dazinator.Extensions.Logging.Tests
{
    public partial class SwitchLogLevelLoggerTests
    {

        [Fact]
        public void Can_Adjust_LogLevel()
        {

            var testLogSink = new TestSink();
            var loggerProvider = new TestLoggerProvider(testLogSink);

            var logProviderContext = SwitchLogLevelLoggerProviderFactory.CreateLogger((a) =>
            {
                a.OwnsInnerProviders = true;
                a.AddInnerProvider(loggerProvider);
            }, initialLevel: LogLevel.Information);

            var services = new ServiceCollection();
            services.AddLogging(b =>
            {
                logProviderContext.Register(b);                
            });

            var newSp = services.BuildServiceProvider();
            var logger = newSp.GetRequiredService<ILogger<SwitchLogLevelLoggerTests>>();

            // Assert
            logger.LogDebug("This is a DEBUG message you won't see this because switch set by default to LogLevel.Information");
            var writes = testLogSink.Writes;
            Assert.Empty(writes);

            var adjustableSwitch = newSp.GetRequiredService<ILoggingLevelSwitch>();
            adjustableSwitch.MinimumLevel = LogLevel.Debug;

            logger.LogDebug("You should see this now!");
            Assert.Single(writes);
        }

        [Fact]
        public void Can_Use_WithScopes()
        {
            var testLogSink = new TestSink();
            var loggerProvider = new TestLoggerProvider(testLogSink);

            var logProviderContext = SwitchLogLevelLoggerProviderFactory.CreateLogger((a) =>
            {
                a.OwnsInnerProviders = true;
                a.AddInnerProvider(loggerProvider);
            }, initialLevel: LogLevel.Information);

            var services = new ServiceCollection();
            services.AddLogging(b =>
            {
                logProviderContext.Register(b);
            });

            var newSp = services.BuildServiceProvider();
            var logger = newSp.GetRequiredService<ILogger<SwitchLogLevelLoggerTests>>();

            var adjustableSwitch = newSp.GetRequiredService<ILoggingLevelSwitch>();
            adjustableSwitch.MinimumLevel = LogLevel.Debug;

            logger.LogDebug("Before Scope 1");
            var writes = testLogSink.Writes;
            Assert.Equal(1, writes.Count);

            using (logger.BeginScope("Scope 1"))
            {
                // Scope is "Checking mail"
                logger.LogInformation("Scope 1 - Info");
                Assert.Equal(2, writes.Count);
                logger.LogDebug("Scope 1 - Debug");
                Assert.Equal(3, writes.Count);

                using (logger.BeginScope("Scope 2"))
                {
                    // Scope is "Checking mail" -> "Downloading messages"
                    logger.LogError("Scope 2 - error");
                    logger.LogInformation("Scope 2 - info");
                    logger.LogDebug("Scope 2 - debug");
                    Assert.Equal(6, writes.Count);
                }
            }

            logger.LogDebug("No Scope.");
            Assert.Equal(7, writes.Count);
            Assert.Null(writes.ToArray()[6].Scope);
        }

        [Fact]
        public void Can_Use_NoOpDiagnosticContext()
        {

            var testLogSink = new TestSink();
            var loggerProvider = new TestLoggerProvider(testLogSink);

            var logProviderContext = SwitchLogLevelLoggerProviderFactory.CreateLogger((a) =>
            {
                a.OwnsInnerProviders = true;
                a.AddInnerProvider(loggerProvider);
            }, initialLevel: LogLevel.Information, addRequestDiagnostics: true);

            var services = new ServiceCollection();
            services.AddLogging(b =>
            {
                logProviderContext.Register(b);
            });

            var newSp = services.BuildServiceProvider();

            var diagnosticContext = newSp.GetRequiredService<IRequestDiagnosticLogContext>();
            diagnosticContext.SetProperty("FOO", "BAR"); // will no-op unless using a logger like serilog that adds request diagnostics capability.
                
       }
    }
}
