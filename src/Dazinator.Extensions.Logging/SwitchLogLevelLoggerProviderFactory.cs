using System;

namespace Microsoft.Extensions.Logging
{
    public static class SwitchLogLevelLoggerProviderFactory
    {
        public static SwitchLogLevelLoggerProviderRegistrationContext<SwitchLogLevelLoggerProvider> CreateLogger(Action<SwitchLogLevelLoggerProviderBuilder> configure, LogLevel initialLevel, bool ownsInnerProviders = false, bool addRequestDiagnostics = false)
        {
            var builder = new SwitchLogLevelLoggerProviderBuilder(ownsInnerProviders);    
            configure(builder);

            var logLevelSwitch = new LoggingLevelSwitch() { MinimumLevel = initialLevel };
            var provider = builder.Build(logLevelSwitch);

            return new SwitchLogLevelLoggerProviderRegistrationContext<SwitchLogLevelLoggerProvider>(provider, logLevelSwitch, addRequestDiagnostics);
        }
    }

}
