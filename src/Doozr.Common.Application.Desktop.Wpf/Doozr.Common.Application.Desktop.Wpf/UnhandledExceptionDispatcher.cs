using System;
using System.Threading.Tasks;

namespace Doozr.Common.Application.Desktop.Wpf
{
	public class UnhandledExceptionDispatcher : IUnhandledExceptionDispatcher
	{
		public event Action<Exception> Handler;

		public UnhandledExceptionDispatcher()
		{
			System.Windows.Application.Current.DispatcherUnhandledException += ApplicationDispatcherUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
		}

		private void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			Handler?.Invoke(e.Exception);
			Environment.Exit(1);
		}

		private void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Handler?.Invoke(e.ExceptionObject as Exception);
			Environment.Exit(1);
		}

		private void ApplicationDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			Handler?.Invoke(e.Exception);
			e.Handled = true;
			System.Windows.Application.Current.Shutdown(1);
		}
	}
}
