using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Logging
{
    public class CompositeLoggerProvider : ILoggerProvider, IDisposable
    {
        private readonly IList<ILoggerProvider> _innerProviders;
        private readonly Predicate<LogLevel> _isEnabled;
        private readonly bool _ownsInnerProviders;
        private readonly IDisposable _innerProvidersDisposable;

        public CompositeLoggerProvider(IList<ILoggerProvider> innerProviders, Predicate<LogLevel> isEnabled, bool ownsInnerProviders = false)
        {
            _innerProviders = innerProviders;
            _isEnabled = isEnabled;
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

        }

        public ILogger CreateLogger(string categoryName)
        {
            var count = _innerProviders.Count;

            var loggers = new ILogger[count];

            for (int i = 0; i < count; i++)
            {
                loggers[i] = _innerProviders[i].CreateLogger(categoryName);
            }
            return new CompositeLogger(_isEnabled, loggers);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_ownsInnerProviders)
                    {
                        _innerProvidersDisposable?.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CompositeLoggerProvider()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
