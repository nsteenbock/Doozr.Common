using System;

namespace Doozr.Common.Logging
{
	public interface ILogManager
	{
		ILogger GetLogger<T>();

		ILogger GetLogger(Type type);
	}
}
