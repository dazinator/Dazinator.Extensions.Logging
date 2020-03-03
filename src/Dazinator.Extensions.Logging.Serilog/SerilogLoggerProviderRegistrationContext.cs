using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Serilog;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;

namespace Microsoft.Extensions.Logging
{
    public class SerilogLoggerProviderRegistrationContext : SwitchLogLevelLoggerProviderRegistrationContext<SerilogLoggerProvider>
    {

        public SerilogLoggerProviderRegistrationContext(Logger logger, bool providerOwnsLogger, global::Serilog.Core.LoggingLevelSwitch minLogLevelSwitch, bool addDiagnosticContext) : base(new SerilogLoggerProvider(logger, providerOwnsLogger), minLogLevelSwitch.ToAdjustableSwitch(), addDiagnosticContext)
        {
            Logger = logger;
        }

        public global::Serilog.ILogger Logger { get; set; }

        protected override void RegisterRequestDiagnostics(IServiceCollection services)
        {
            var diagnosticContext = new DiagnosticContext(Logger);

            // Consumed by e.g. middleware
            services.AddSingleton(diagnosticContext);

            // Consumed by user code
            services.AddSingleton<IDiagnosticContext>(diagnosticContext);
            services.AddSingleton<IRequestDiagnosticLogContext, SerilogRequestDiagnosticLogContext>();
        }
    }
}
