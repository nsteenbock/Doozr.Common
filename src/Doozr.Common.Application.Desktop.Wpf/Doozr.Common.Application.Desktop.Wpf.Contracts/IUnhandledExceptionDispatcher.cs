using System;

namespace Doozr.Common.Application.Desktop.Wpf
{
	public interface IUnhandledExceptionDispatcher
	{
		event Action<Exception> Handler;
	}
}
