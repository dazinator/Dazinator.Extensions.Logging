namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// When the underlying logger supports request diagnostics, this interface let's you add
    /// properties to the log that is sent after the request is processed that includes the request diagnostic information.
    /// </summary>
    public interface IRequestDiagnosticLogContext
    {
        /// <summary>
        /// If the underlying logger supports request diagnostics, this will append a property value to the log that contains the diagnostics, which gets generated after the request is processed.
        /// If the underlying logger doesn't support this, then this may No-Op.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRequestDiagnosticLogContext SetProperty(string name, object value);
    }
}