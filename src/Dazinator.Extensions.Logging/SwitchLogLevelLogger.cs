using System;

namespace Microsoft.Extensions.Logging
{
    public class SwitchLogLevelLogger : CompositeLogger
    {
        public SwitchLogLevelLogger(ILoggingLevelSwitch logLevelSwitch, ILogger[] innerLoggers) : base((logLevel) =>
        {
            return logLevel >= logLevelSwitch.MinimumLevel;
        }, innerLoggers)
        {
        }
    }
}