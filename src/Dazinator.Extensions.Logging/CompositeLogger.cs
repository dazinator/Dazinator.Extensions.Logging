using System;

namespace Microsoft.Extensions.Logging
{
    public class CompositeLogger : ILogger
    {
        private readonly Predicate<LogLevel> _isEnabled;
        private ILogger[] _innerLoggers;

        public CompositeLogger(Predicate<LogLevel> isEnabled, ILogger[] innerLoggers)
        {
            _isEnabled = isEnabled;
            _innerLoggers = innerLoggers;
        }

        // if your logger uses unmanaged resources, you can
        // return the class that implements IDisposable here
        public IDisposable BeginScope<TState>(TState state)
        {
            var disposables = new IDisposable[_innerLoggers.Length];
            for (int i = 0; i < _innerLoggers.Length; i++)
            {
                disposables[i] = _innerLoggers[i].BeginScope<TState>(state);
            }
            return new CompositeDisposable(disposables);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // to avoid overlogging, you can filter
            // on the log level
            return _isEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(IsEnabled(logLevel))
            {
                foreach (var innerLogger in _innerLoggers)
                {
                    innerLogger.Log(logLevel, eventId, state, exception, formatter);
                }
            }           
        }
    }
}