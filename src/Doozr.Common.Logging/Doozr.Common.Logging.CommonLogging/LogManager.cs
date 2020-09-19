using System;

namespace Doozr.Common.Logging.CommonLogging
{
	public class LogManager : Doozr.Common.Logging.LogManager
	{
		public override ILogger GetLogger(Type type)
		{
			return new Logger(global::Common.Logging.LogManager.GetLogger(type));
		}
	}
}
