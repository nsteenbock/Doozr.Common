using System;

namespace Doozr.Common.Logging
{
	public abstract class LogManager : ILogManager
	{
		public abstract ILogger GetLogger(Type type);

		public ILogger GetLogger<T>()
		{
			return GetLogger(typeof(T));
		}
	}
}
