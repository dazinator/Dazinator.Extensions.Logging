namespace Microsoft.Extensions.Logging
{
    public interface ILoggingLevelSwitch
    {
        LogLevel MinimumLevel { get; set; }
    }
}