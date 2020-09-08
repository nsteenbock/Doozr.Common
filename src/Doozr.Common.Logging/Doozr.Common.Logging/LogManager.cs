namespace Doozr.Common.Logging
{
	public abstract class LogManager : ILogManager
	{
		public abstract ILogger GetLogger<T>();
	}
}
