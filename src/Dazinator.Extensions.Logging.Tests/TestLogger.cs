using Microsoft.Extensions.Logging;
using System;

namespace Dazinator.Extensions.Logging.Tests
{
    public class TestLogger : ILogger
    {
        private object _scope;
        private readonly ITestSink _sink;
        private readonly string _name;
        private readonly Func<LogLevel, bool> _filter;

        public TestLogger(string name, ITestSink sink, bool enabled)
            : this(name, sink, _ => enabled)
        {
        }

        public TestLogger(string name, ITestSink sink, Func<LogLevel, bool> filter)
        {
            _sink = sink;
            _name = name;
            _filter = filter;
        }

        public string Name { get; set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            var oldScope = _scope;
            var disposable = new DelegateDisposable<TState>(() =>
            {
                _scope = oldScope;
            });

            _scope = state;

            _sink.Begin(new BeginScopeContext()
            {
                LoggerName = _name,
                Scope = state,
            });

            return disposable;

        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            _sink.Write(new WriteContext()
            {
                LogLevel = logLevel,
                EventId = eventId,
                State = state,
                Exception = exception,
                Formatter = (s, e) => formatter((TState)s, e),
                LoggerName = _name,
                Scope = _scope
            });
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None && _filter(logLevel);
        }

        private class TestDisposable : IDisposable
        {
            public static readonly TestDisposable Instance = new TestDisposable();

            public TestDisposable()
            {
            }
            public void Dispose()
            {
                // intentionally does nothing
            }
        }

        private class DelegateDisposable<TScope> : IDisposable
        {
            private readonly Action _onDispose;

            public DelegateDisposable(Action onDispose)
            {
                _onDispose = onDispose;
            }
            public void Dispose()
            {
                _onDispose?.Invoke();
            }
        }
    }

}
