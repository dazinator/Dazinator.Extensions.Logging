using System.Collections.Generic;

namespace Microsoft.Extensions.Logging
{
    public class AdjustableLoggerProviderBuilder
    {
        private ILoggingBuilder _builder;
        private readonly ILoggingLevelSwitch _loggingLevelSwitch;
        private List<ILoggerProvider> _innerProviders = new List<ILoggerProvider>();

        public AdjustableLoggerProviderBuilder(ILoggingBuilder builder, ILoggingLevelSwitch loggingLevelSwitch, bool ownsInnerProviders = false)
        {
            _builder = builder;
            _loggingLevelSwitch = loggingLevelSwitch;
            OwnsInnerProviders = ownsInnerProviders;
        }

        public AdjustableLoggerProviderBuilder AddInnerProvider(ILoggerProvider provider)
        {
            _innerProviders.Add(provider);
            return this;
        }

        public bool OwnsInnerProviders { get; set; }

        public IAdjustableLogLevelLoggerProvider Build()
        {
            var provider = new AdjustableLogLevelLoggerProvider(_innerProviders, _loggingLevelSwitch, OwnsInnerProviders);
            return provider;
        }
    }
}