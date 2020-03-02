using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class LoggingBuilderExtensions
    {

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
    }
}