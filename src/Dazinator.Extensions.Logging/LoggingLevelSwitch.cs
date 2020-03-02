namespace Microsoft.Extensions.Logging
{
    public class LoggingLevelSwitch : ILoggingLevelSwitch
    {
        public LogLevel MinimumLevel { get; set; }
    }
}