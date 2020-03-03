namespace Microsoft.Extensions.Logging
{
    public interface IRequestDiagnosticLogContext
    {
        IRequestDiagnosticLogContext SetProperty(string name, object value);
    }
}