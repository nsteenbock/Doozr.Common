namespace Doozr.Common.Logging.CommonLogging
{
	public class LogManager : Doozr.Common.Logging.LogManager
	{
		public override ILogger GetLogger<T>()
		{
			return new Logger(global::Common.Logging.LogManager.GetLogger<T>());
		}
	}
}
