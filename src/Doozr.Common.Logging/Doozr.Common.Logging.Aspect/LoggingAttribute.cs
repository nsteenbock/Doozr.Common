using MethodBoundaryAspect.Fody.Attributes;

namespace Doozr.Common.Logging.Aspect
{
	public class LoggingAttribute: OnMethodBoundaryAspect
	{
		public static ILogManager LogManager{ get; set; }

		public override void OnEntry(MethodExecutionArgs arg)
		{
			if (arg.Method.Name == "set_Logger" || arg.Method.Name == "get_Logger") return;
			var loggingInstance = (arg.Instance as ILoggingObject);
			var logger = loggingInstance?.Logger ?? LogManager?.GetLogger(arg.Instance.GetType());
			logger?.EnterMethod(arg.Method.Name);
		}

		public override void OnExit(MethodExecutionArgs arg)
		{
			if (arg.Method.Name == "set_Logger" || arg.Method.Name == "get_Logger") return;
			var loggingInstance = (arg.Instance as ILoggingObject);
			var logger = loggingInstance?.Logger ?? LogManager?.GetLogger(arg.Instance.GetType());
			logger?.LeaveMethod(arg.Method.Name);
		}

		public override void OnException(MethodExecutionArgs arg)
		{
			if (arg.Method.Name == "set_Logger" || arg.Method.Name == "get_Logger") return;
			var loggingInstance = (arg.Instance as ILoggingObject);
			var logger = loggingInstance?.Logger ?? LogManager?.GetLogger(arg.Instance.GetType());
			logger?.LogException(arg.Exception);
			logger?.LeaveMethod(arg.Method.Name);
		}
	}
}
