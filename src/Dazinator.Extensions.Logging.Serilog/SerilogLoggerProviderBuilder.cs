using Microsoft.Extensions.DependencyInjection;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using System.Globalization;

namespace Microsoft.Extensions.Logging.Serilog
{
    public class SerilogLoggerProviderBuilder
    {
        private readonly ILoggingLevelSwitch _loggingLevelSwitch;
        private global::Serilog.ILogger _logger;

        public SerilogLoggerProviderBuilder()
        {
        }

        public SerilogLoggerProviderBuilder SetLogger(global::Serilog.ILogger logger, bool ownsLogger = false)
        {
            _logger = logger;
            OwnsLogger = ownsLogger;
            return this;
        }

        public SerilogLoggerProviderBuilder SetSwitch(global::Serilog.Core.LoggingLevelSwitch minimumLevelSwitch)
        {
            LogLevelSwitch = minimumLevelSwitch.ToAdjustableSwitch();
            return this;
        }

        public SerilogLoggerProviderBuilder SetSwitch(ILoggingLevelSwitch logLevelSwitch)
        {
            LogLevelSwitch = logLevelSwitch;
            return this;
        }

        public bool OwnsLogger { get; set; }

        public bool AddDiagnosticContext { get; set; }
        public ILoggingLevelSwitch LogLevelSwitch { get; set; }

        public global::Serilog.ILogger Logger { get; set; }

        public SerilogLoggerProviderBuilder AddRequestDiagnosticContext()
        {
            AddDiagnosticContext = true;
            return this;
        }

        public ILoggerProvider Build()
        {
            var provider = new SerilogLoggerProvider(_logger, OwnsLogger);
            return provider;
        }
    }


}
