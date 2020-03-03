using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Serilog;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;

namespace Microsoft.Extensions.Logging
{

    public static class SerilogBuilderExtensions
    {
        public static ILoggingLevelSwitch ToAdjustableSwitch(this global::Serilog.Core.LoggingLevelSwitch serilogSwitch)
        {
            var adjustableSwitch = new SerilogLoggingLevelSwitch(serilogSwitch);
            return adjustableSwitch;
        }

        public static IServiceCollection AddSerilogRequestDiagnosticContext(this IServiceCollection services, global::Serilog.ILogger logger)
        {
            var diagnosticContext = new DiagnosticContext(logger);

            // Consumed by e.g. middleware
            services.AddSingleton(diagnosticContext);

            // Consumed by user code
            services.AddSingleton<IDiagnosticContext>(diagnosticContext);
            services.AddSingleton<IRequestDiagnosticLogContext, SerilogRequestDiagnosticLogContext>();

            return services;
        }

        public static LoggerConfiguration Provider(this LoggerSinkConfiguration configuration, ILoggerProvider provider, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch levelSwitch = null)
        {
            var collection = new LoggerProviderCollection();
            collection.AddProvider(provider);
            return configuration.Providers(collection);
        }
    }
}