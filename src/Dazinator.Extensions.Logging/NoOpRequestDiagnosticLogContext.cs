namespace Microsoft.Extensions.Logging
{
    public class NoOpRequestDiagnosticLogContext : IRequestDiagnosticLogContext
    {
        public IRequestDiagnosticLogContext SetProperty(string name, object value)
        {
            // intentional no-op;     
            return this;
        }
    }
}