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

        ///// <summary>
        /////     Push a property onto the context, returning an System.IDisposable that must later
        /////     be used to remove the property, along with any others that may have been pushed
        /////     on top of it and not yet popped. The property must be popped from the same thread/logical
        /////     call context.
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public IDisposable PushProperty(string name, object value)
        //{
        //    return LogContext.PushProperty(name, value);
        //}

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
