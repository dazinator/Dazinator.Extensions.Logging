namespace Dazinator.Extensions.Logging.Tests
{
    public class BeginScopeContext
    {
        public object Scope { get; set; }

        public string LoggerName { get; set; }
    }

    public class EndScopeContext
    {
        public object Scope { get; set; }

        public string LoggerName { get; set; }
    }

}
