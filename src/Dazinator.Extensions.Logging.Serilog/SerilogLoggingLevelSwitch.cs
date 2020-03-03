using Serilog.Context;
using Serilog.Events;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Logging.Serilog
{
    public class SerilogLoggingLevelSwitch : ILoggingLevelSwitch
    {
        private readonly global::Serilog.Core.LoggingLevelSwitch _logLevelSwitch;

        public SerilogLoggingLevelSwitch(global::Serilog.Core.LoggingLevelSwitch logLevelSwitch)
        {
            _logLevelSwitch = logLevelSwitch;
        }

        public LogLevel MinimumLevel
        {
            get
            {
                switch (_logLevelSwitch.MinimumLevel)
                {
                    case LogEventLevel.Debug:
                        return LogLevel.Debug;
                    case LogEventLevel.Error:
                        return LogLevel.Error;
                    case LogEventLevel.Fatal:
                        return LogLevel.Critical;
                    case LogEventLevel.Information:
                        return LogLevel.Information;
                    case LogEventLevel.Verbose:
                        return LogLevel.Trace;
                    case LogEventLevel.Warning:
                        return LogLevel.Warning;
                    default:
                        return LogLevel.Trace;
                }
            }
            set
            {
                _logLevelSwitch.MinimumLevel = ToSerilogLogLevel(value);
            }
        }

        private LogEventLevel ToSerilogLogLevel(LogLevel level)
        {
            switch (level)
            {
                // as there is no match for 'None' in Serilog, pick the least logging possible
                case LogLevel.None:
                case LogLevel.Critical:
                    return LogEventLevel.Fatal;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Trace:
                default:
                    return LogEventLevel.Verbose;
            }
        }
    }


}
