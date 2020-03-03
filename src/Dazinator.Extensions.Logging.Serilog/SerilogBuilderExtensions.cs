using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.Logging.Serilog
{
    public static class SerilogBuilderExtensions
    {
        //public AdjustableLoggerProviderBuilder AddSerilog(this AdjustableLoggerProviderBuilder builder, Action<LoggerConfiguration> configureSerilog)
        //{
        //    var loggerConfig = new LoggerConfiguration();
        //    configureSerilog(loggerConfig);
        //    return builder;
        //         //.MinimumLevel.ControlledBy(loggingLevelSwitch);
        //         // Registered to provide two services...

        //}

        public static ILoggingBuilder AddAdjustableLoggerProvider(this ILoggingBuilder builder, LogLevel initialMinimumLogLevel, Action<AdjustableLoggerProviderBuilder> configure)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var logLevelSwitch = new LoggingLevelSwitch() { MinimumLevel = initialMinimumLogLevel };
            var adjustableLoggerBuilder = new AdjustableLoggerProviderBuilder(builder, logLevelSwitch);
            configure?.Invoke(adjustableLoggerBuilder);
            var provider = adjustableLoggerBuilder.Build();
            builder.AddProvider(provider);
            builder.Services.AddSingleton<ILoggingLevelSwitch>(provider.LogLevelSwitch);
            builder.AddFilter<AdjustableLogLevelLoggerProvider>(null, LogLevel.Trace); // because it's dynamic we always need to get all log events.
            return builder;
        }

        public static ILoggingBuilder AddAdjustableLoggerProvider<TSwitch>(this ILoggingBuilder builder, TSwitch logLevelSwitch, Action<AdjustableLoggerProviderBuilder> configure)
            where TSwitch : ILoggingLevelSwitch
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var adjustableLoggerBuilder = new AdjustableLoggerProviderBuilder(builder, logLevelSwitch);
            configure?.Invoke(adjustableLoggerBuilder);
            var provider = adjustableLoggerBuilder.Build();
            builder.AddProvider(provider);
            builder.Services.AddSingleton<ILoggingLevelSwitch>(provider.LogLevelSwitch);
            return builder;
        }




        public static AdjustableLoggerProviderBuilder AddSerilogRequestDiagnosticContext(this AdjustableLoggerProviderBuilder builder, global::Serilog.ILogger logger)
        {
            var diagnosticContext = new DiagnosticContext(logger);

            // Consumed by e.g. middleware
            builder.Services.AddSingleton(diagnosticContext);

            // Consumed by user code
            builder.Services.AddSingleton<IDiagnosticContext>(diagnosticContext);
            builder.Services.AddSingleton<IRequestDiagnosticLogContext, SerilogRequestDiagnosticLogContext>();

            return builder;
        }

    }


}
