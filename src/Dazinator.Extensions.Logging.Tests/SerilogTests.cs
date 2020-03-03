using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Xunit;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Dazinator.Extensions.Logging.Tests
{
    public partial class SerilogTests
    {

        [Fact]
        public void Can_Adjust_LogLevel()
        {

            var testLogSink = new TestSink();
            var innerProvider = new TestLoggerProvider(testLogSink);
            var serilogLogger = CreateSerilogLogger(innerProvider);
            var loggerProvider = new SerilogLoggerProvider(serilogLogger);

            var services = new ServiceCollection();
            services.AddLogging(b =>
            {                
                b.AddAdjustableLoggerProvider(LogLevel.Information, (l) =>
                {                    
                    l.AddInnerProvider(loggerProvider);
                });
            });

            var newSp = services.BuildServiceProvider();
            var logger = newSp.GetRequiredService<ILogger<AdjustableLogLevelLoggerTests>>();

            // Assert
            logger.LogDebug("This is a DEBUG message you won't see this because switch set by default to LogLevel.Information");
            var writes = testLogSink.Writes;
            Assert.Empty(writes);

            var adjustableSwitch = newSp.GetRequiredService<ILoggingLevelSwitch>();
            adjustableSwitch.MinimumLevel = LogLevel.Debug;

            logger.LogDebug("You should see this now!");
            Assert.Single(writes);
        }


        private global::Serilog.ILogger CreateSerilogLogger(ILoggerProvider innerProvider)
        {

            // var loggingLevelSwitch = new global::Serilog.Core.LoggingLevelSwitch(LogEventLevel.Verbose); // we are platform level provider, and tenant level logs are forwarded through us controlled by their own log level switches, so we set to verbose here so we don't block them.
            var innerProviders = new LoggerProviderCollection();
            innerProviders.AddProvider(innerProvider);

            var loggerConfig = new LoggerConfiguration()
                   .MinimumLevel.Verbose()
                   //.ControlledBy(loggingLevelSwitch)
                 //.ReadFrom.Configuration(configuration)
                 // .Enrich.FromLogContext()
                 //.Enrich.WithProperty("EnvironmentTag", envNameValue)
                 //.Enrich.WithProperty("EnvironmentName", aspnetCoreEnvironment)
                 //.Enrich.WithProperty("RuntimeFramework", runtimeFramework)
                 //.Enrich.WithMachineName()
                 //.Enrich.WithUserName()
                 //.Enrich.WithAssemblyName()
                 //.Enrich.WithAssemblyVersion()
                 .Enrich.FromLogContext()
                 .WriteTo.Providers(innerProviders);

            //if (_writeToProviders != null)
            //{
            //    loggerConfig = loggerConfig.WriteTo.Providers(_writeToProviders);
            //}
            // .Enrich.WithProperty("ApplicationBasePath", basePath)
            var logger = loggerConfig.CreateLogger();
            return logger;

            //if (SetStaticSerilogLogger)
            //{
            //    Log.Logger = logger;
            //}
        }


    }
}
