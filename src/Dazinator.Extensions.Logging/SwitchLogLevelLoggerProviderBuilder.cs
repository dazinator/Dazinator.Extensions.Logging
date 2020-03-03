using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging
{
    public class SwitchLogLevelLoggerProviderBuilder
    {
        private List<ILoggerProvider> _innerProviders = new List<ILoggerProvider>();

        public SwitchLogLevelLoggerProviderBuilder(bool ownsInnerProviders = false)
        {
            OwnsInnerProviders = ownsInnerProviders;
        }

        public SwitchLogLevelLoggerProviderBuilder AddInnerProvider(ILoggerProvider provider)
        {
            _innerProviders.Add(provider);
            return this;
        }

        public bool OwnsInnerProviders { get; set; }

        public ILoggerProvider Build(ILoggingLevelSwitch logLevelSwitch)
        {
            var provider = new SwitchLogLevelLoggerProvider(_innerProviders, logLevelSwitch, OwnsInnerProviders);
            return provider;
        }

    }
}