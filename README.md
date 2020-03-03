## Features
An `ILoggerProvider` that can allow you to dynamically adjust the minimum `LogLevel` at runtime.

This provider just wraps some inner providers, but only forwards events if the event meets the current Loglevel which is adjustable.

See the tests for usage. Here is a quick example:


```csharp

            var services = new ServiceCollection();
            services.AddLogging(b =>
            {
                // Add the provider, default the switch to min log level: Information.
                b.AddAdjustableLoggerProvider(LogLevel.Information, (l) =>
                {
                    l.AddInnerProvider(loggerProvider); // add your inner ILogProvider/s such as Console etc.
                });
            });

            var newSp = services.BuildServiceProvider();
            var logger = newSp.GetRequiredService<ILogger<Startup>>();

            logger.LogDebug("This is a DEBUG message you won't see this because switch currently set by default to LogLevel.Information");

            // This is how you adjust the min log level at runtime.. - via `ILoggingLevelSwitch`
            var adjustableSwitch = newSp.GetRequiredService<ILoggingLevelSwitch>();
            adjustableSwitch.MinimumLevel = LogLevel.Debug;

            logger.LogDebug("You should see this now!");

```
