## Features
An `ILogProvider` that can allow you to dynamically adjust the minimum `LogLevel` at runtime.

This provider just wraps some inner providers, but only forwards events if the event meets the current Loglevel which is adjustable.

See the tests for usage. Here is a quick example:


```csharp

            // Initialise the logger early in your program and use it before DI, but hold onto the context to register it with DI later.           
            var logProviderContext = SwitchLogLevelLoggerProviderFactory.CreateLogger((a) =>
            {
                a.AddInnerProvider(loggerProvider);
            }, initialLevel: LogLevel.Information);            

            // Start logging!
            var logger = logProviderContext.LoggerProvider.CreateLogger("Startup");
            logger.LogInformation("Started");

            // Then later when configuring DI..
            var services = new ServiceCollection();
            services.AddLogging(b =>
            {
                logProviderContext.Register(b);
            });          

            // Now you can inject ILogger<T> as normal.
            var newSp = services.BuildServiceProvider();
            var logger = newSp.GetRequiredService<ILogger<Startup>>();

            logger.LogDebug("This is a DEBUG message you won't see this because switch currently set by default to LogLevel.Information");

            // This is how you adjust the min log level at runtime.. - via `ILoggingLevelSwitch`
            var adjustableSwitch = newSp.GetRequiredService<ILoggingLevelSwitch>();
            adjustableSwitch.MinimumLevel = LogLevel.Debug;
            // or keep a reference to logProviderContext.LogLevelSwitch and change it that way if you can't use DI.


            logger.LogDebug("You should see this now!");

```