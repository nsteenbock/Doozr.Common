namespace Doozr.Common.Logging
{
	public interface ILogManager
	{
		ILogger GetLogger<T>();
	}
}
