using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging
{
    public class AdjustableLogLevelLoggerProvider : IAdjustableLogLevelLoggerProvider, IDisposable
    {
        private readonly IList<ILoggerProvider> _innerProviders;
        private readonly bool _ownsInnerProviders;
        private readonly IDisposable _innerProvidersDisposable;

        public AdjustableLogLevelLoggerProvider(IList<ILoggerProvider> innerProviders, ILoggingLevelSwitch logLevelSwitch, bool ownsInnerProviders = false)
        {
            _innerProviders = innerProviders;
            _ownsInnerProviders = ownsInnerProviders;

            if (ownsInnerProviders)
            {
                var disposables = new IDisposable[_innerProviders.Count];
                for (int i = 0; i < _innerProviders.Count; i++)
                {
                    var provider = _innerProviders[i];
                    if (provider is IDisposable)
                    {
                        disposables[i] = (IDisposable)provider;
                    }
                    else
                    {
                        disposables[i] = null;
                    }
                }

                _innerProvidersDisposable = new CompositeDisposable(disposables);
            }

            LogLevelSwitch = logLevelSwitch;
        }

        public ILoggingLevelSwitch LogLevelSwitch { get; }

        public ILogger CreateLogger(string categoryName)
        {
            var count = _innerProviders.Count;

            var loggers = new ILogger[count];

            for (int i = 0; i < count; i++)
            {
                loggers[i] = _innerProviders[i].CreateLogger(categoryName);
            }
            return new AdjustableLogLevelLogger(LogLevelSwitch, loggers);
        }

        public void Dispose()
        {
            if (_ownsInnerProviders)
            {
                _innerProvidersDisposable?.Dispose();
            }
        }
    }
}
