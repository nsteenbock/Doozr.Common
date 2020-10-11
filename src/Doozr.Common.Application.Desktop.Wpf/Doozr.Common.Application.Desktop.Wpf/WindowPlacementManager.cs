using Doozr.Common.Application.Desktop.Window.Interop;
using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System;
using System.Windows.Interop;

namespace Doozr.Common.Application.Desktop.Wpf
{
	[Log]
	public class WindowPlacementManager: IWindowPlacementManager, ILoggingObject
	{
		public ILogger Logger { get; set; }

		public string GetWindowPlacement(System.Windows.Window window)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			var result = WindowPlacementFunctions.GetPlacement(handle);

			Logger?.LogString("result", result);
			
			return result;
		}

		public void SetWindowPlacement(System.Windows.Window window, string placement)
		{
			Logger?.LogString("placement", placement);

			IntPtr handle = new WindowInteropHelper(window).Handle;
			WindowPlacementFunctions.SetPlacement(handle, placement);
		}
	}
}
