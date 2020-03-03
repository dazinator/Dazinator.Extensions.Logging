using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging
{
    public class SwitchLogLevelLoggerProvider : CompositeLoggerProvider
    {
        public SwitchLogLevelLoggerProvider(IList<ILoggerProvider> innerProviders, ILoggingLevelSwitch logLevelSwitch, bool ownsInnerProviders = false) : base(innerProviders, (logLevel) =>
          {
              return logLevel >= logLevelSwitch.MinimumLevel;
          }, ownsInnerProviders)
        {
        }
    }
}
