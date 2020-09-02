using System.Windows;

namespace Doozr.Common.Application.Desktop.Wpf
{
	public interface IWindowPlacementManager
	{
		string GetWindowPlacement(Window window);

		void SetWindowPlacement(Window window, string placement);
	}
}
