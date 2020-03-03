using Serilog;

namespace Microsoft.Extensions.Logging.Serilog
{
    public class SerilogRequestDiagnosticLogContext : IRequestDiagnosticLogContext
    {
        private readonly IDiagnosticContext _context;
        private readonly bool _destructureObjects;

        public SerilogRequestDiagnosticLogContext(IDiagnosticContext context, bool destructureObjects = false)
        {
            _context = context;
            _destructureObjects = destructureObjects;
        }

        /// <summary>
        /// Summary:
        ///     Set the specified property on the current diagnostic context. The property will
        ///     be collected and attached to the event emitted at the completion of the context.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IRequestDiagnosticLogContext SetProperty(string name, object value)
        {
            _context.Set(name, value, _destructureObjects);
            return this;
        }
    }


}
