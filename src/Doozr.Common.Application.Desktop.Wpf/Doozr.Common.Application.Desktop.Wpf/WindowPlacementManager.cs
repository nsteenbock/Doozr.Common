using Doozr.Common.Application.Desktop.Window.Interop;
using System;
using System.Windows.Interop;

namespace Doozr.Common.Application.Desktop.Wpf
{
	public class WindowPlacementManager: IWindowPlacementManager
	{
		public string GetWindowPlacement(System.Windows.Window window)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			return WindowPlacementFunctions.GetPlacement(handle);
		}

		public void SetWindowPlacement(System.Windows.Window window, string placement)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			WindowPlacementFunctions.SetPlacement(handle, placement);
		}
	}
}
