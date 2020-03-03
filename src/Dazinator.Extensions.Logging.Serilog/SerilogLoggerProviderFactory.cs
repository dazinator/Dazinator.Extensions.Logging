using Serilog;
using Serilog.Events;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class SerilogLoggerProviderFactory
    {
        public static SerilogLoggerProviderRegistrationContext CreateLogger(Action<LoggerConfiguration> configure, bool providerOwnsLogger = false, bool addDiagnosticContext = false)
        {
            var minLogLevelSwitch = new global::Serilog.Core.LoggingLevelSwitch(LogEventLevel.Verbose); // we are platform level provider, and tenant level logs are forwarded through us controlled by their own log level switches, so we set to verbose here so we don't block them.

            var loggerConfig = new LoggerConfiguration()
                   .MinimumLevel.ControlledBy(minLogLevelSwitch);

            configure(loggerConfig);

            var logger = loggerConfig.CreateLogger();

            return new SerilogLoggerProviderRegistrationContext(logger, providerOwnsLogger, minLogLevelSwitch, addDiagnosticContext);
        }
    }

}
