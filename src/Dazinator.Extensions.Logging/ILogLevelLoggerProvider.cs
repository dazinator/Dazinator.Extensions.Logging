using System.Linq;

namespace Microsoft.Extensions.Logging
{

    public interface ILogLevelLoggerProvider : ILoggerProvider
    {
       
        /// <summary>
        /// A switch that can be used to adjust the minimum log level at runtime.
        /// </summary>
        /// <remarks>Could be null if the underlying logger does not support adjusting the min log level at runtime</remarks>
        ILoggingLevelSwitch LogLevelSwitch { get; }
    }
}