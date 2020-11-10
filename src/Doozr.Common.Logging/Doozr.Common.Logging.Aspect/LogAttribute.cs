using MethodBoundaryAspect.Fody.Attributes;

namespace Doozr.Common.Logging.Aspect
{
	public class LogAttribute: OnMethodBoundaryAspect
	{
		public static ILogManager LogManager{ get; set; }

		public LogLevel LogLevel { get; set; } = LogLevel.Verbose;

		public override void OnEntry(MethodExecutionArgs arg)
		{
			if (arg.Method.Name == "set_Logger" || arg.Method.Name == "get_Logger") return;
			var loggingInstance = (arg.Instance as ILoggingObject);
			var logger = loggingInstance?.Logger ?? LogManager?.GetLogger(arg.Instance.GetType());
			logger?.EnterMethod(LogLevel, arg.Method.Name);
		}

		public override void OnExit(MethodExecutionArgs arg)
		{
			if (arg.Method.Name == "set_Logger" || arg.Method.Name == "get_Logger") return;
			var loggingInstance = (arg.Instance as ILoggingObject);
			var logger = loggingInstance?.Logger ?? LogManager?.GetLogger(arg.Instance.GetType());
			logger?.LeaveMethod(LogLevel, arg.Method.Name);
		}

		public override void OnException(MethodExecutionArgs arg)
		{
			if (arg.Method.Name == "set_Logger" || arg.Method.Name == "get_Logger") return;
			var loggingInstance = (arg.Instance as ILoggingObject);
			var logger = loggingInstance?.Logger ?? LogManager?.GetLogger(arg.Instance.GetType());
			logger?.LogException(arg.Exception);
			logger?.LeaveMethod(LogLevel, arg.Method.Name);
		}
	}
}
