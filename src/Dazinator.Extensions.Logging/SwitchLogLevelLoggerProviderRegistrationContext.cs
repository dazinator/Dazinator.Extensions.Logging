using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
    public class SwitchLogLevelLoggerProviderRegistrationContext<TLoggerProvider>
        where TLoggerProvider : ILoggerProvider
    {
        private readonly bool _registerRequestDiagnostics;

        public SwitchLogLevelLoggerProviderRegistrationContext(ILoggerProvider provider, ILoggingLevelSwitch logLevelSwitch, bool registerRequestDiagnostics)
        {
            LoggerProvider = provider;
            LogLevelSwitch = logLevelSwitch;
            _registerRequestDiagnostics = registerRequestDiagnostics;
        }
        public ILoggingLevelSwitch LogLevelSwitch { get; set; }
        public ILoggerProvider LoggerProvider { get; set; }

        public virtual ILoggingBuilder Register(ILoggingBuilder builder)
        {
            builder.AddProvider(LoggerProvider);

            if (LogLevelSwitch != null)
            {
                builder.Services.AddSingleton(LogLevelSwitch);
            }

            builder.AddFilter<TLoggerProvider>(null, LogLevel.Trace);  // because it's dynamically adjustable the provider always needs to get all log events then it decides.

            if (_registerRequestDiagnostics)
            {
                RegisterRequestDiagnostics(builder.Services);
            }
            return builder;
        }

        protected virtual void RegisterRequestDiagnostics(IServiceCollection services)
        {
            services.AddSingleton<IRequestDiagnosticLogContext, NoOpRequestDiagnosticLogContext>();
        }
    }

}
