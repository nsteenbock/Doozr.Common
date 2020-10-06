using MethodBoundaryAspect.Fody.Attributes;

namespace Doozr.Common.Logging.Aspect
{
	public class LoggingAttribute: OnMethodBoundaryAspect
	{
		public override void OnEntry(MethodExecutionArgs arg)
		{
			var loggingInstance = (arg.Instance as ILoggingObject);
			loggingInstance?.Logger.EnterMethod(arg.Method.Name);
		}

		public override void OnExit(MethodExecutionArgs arg)
		{
			var loggingInstance = (arg.Instance as ILoggingObject);
			loggingInstance?.Logger.LeaveMethod(arg.Method.Name);
		}

		public override void OnException(MethodExecutionArgs arg)
		{
			var loggingInstance = (arg.Instance as ILoggingObject);
			loggingInstance?.Logger.LogException(arg.Exception);
			loggingInstance?.Logger.LeaveMethod(arg.Method.Name);
		}
	}
}
