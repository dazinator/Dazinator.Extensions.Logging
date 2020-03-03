using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Extensions.Hosting;
using System;

namespace Microsoft.Extensions.Logging.Serilog
{
    //public class SerilogLoggerProvider : IAdjustableLogLevelLoggerProvider
    //{
    //    private readonly global::Serilog.ILogger _serilogLogger;
    //    private readonly bool _dispose;
    //    private readonly bool _addNewDiagnosticContext;

    //    public SerilogLoggerProvider(IConfiguration config, global::Serilog.ILogger logger, global::Serilog.Core.LoggingLevelSwitch loggingLevelSwitch, bool dispose = false, bool addNewDiagnosticContext = true)
    //    {
    //        Configuration = config; // the config that the logger was built from.
    //        _serilogLogger = logger;
    //        LogLevelSwitch = new SerilogLoggingLevelSwitch(loggingLevelSwitch); // can be used to dynamically control / switch min log level at runtime.
    //        _dispose = dispose;
    //        _addNewDiagnosticContext = addNewDiagnosticContext;
    //        LazyLoggerProvider = new Lazy<Serilog.Extensions.Logging.SerilogLoggerProvider>(() => new Serilog.Extensions.Logging.SerilogLoggerProvider(_serilogLogger, _dispose));
    //    }

    //    public ILoggingLevelSwitch LogLevelSwitch { get; }


    //    public Lazy<Serilog.Extensions.Logging.SerilogLoggerProvider> LazyLoggerProvider { get; set; }

    //    public IConfiguration Configuration { get; }

    //    public void Register(ILoggingBuilder builder)
    //    {
    //        builder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Trace);

    //        if (_addNewDiagnosticContext)
    //        {
    //            // Registered to provide two services...
    //            var diagnosticContext = new DiagnosticContext(_serilogLogger);

    //            // Consumed by e.g. middleware
    //            builder.Services.AddSingleton(diagnosticContext);

    //            // Consumed by user code
    //            builder.Services.AddSingleton<IDiagnosticContext>(diagnosticContext);
    //            builder.Services.AddSingleton<IRequestDiagnosticLogContext, SerilogRequestDiagnosticLogContext>();

    //        }

    //    }

    //    public ILogger CreateLogger(string categoryName)
    //    {
    //        return LazyLoggerProvider.Value.CreateLogger(categoryName);
    //    }

    //    #region IDisposable Support
    //    private bool disposedValue = false; // To detect redundant calls

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!disposedValue)
    //        {
    //            if (disposing)
    //            {
    //                // TODO: dispose managed state (managed objects).
    //                if (LazyLoggerProvider.IsValueCreated)
    //                {
    //                    LazyLoggerProvider.Value.Dispose();
    //                }
    //            }

    //            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
    //            // TODO: set large fields to null.

    //            disposedValue = true;
    //        }
    //    }

    //    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    //    // ~SerilogLoggerProvider()
    //    // {
    //    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //    //   Dispose(false);
    //    // }

    //    // This code added to correctly implement the disposable pattern.
    //    public void Dispose()
    //    {
    //        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //        Dispose(true);
    //        // TODO: uncomment the following line if the finalizer is overridden above.
    //        // GC.SuppressFinalize(this);
    //    }
    //    #endregion


    //}


}
