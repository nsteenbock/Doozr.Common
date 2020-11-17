using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System.Windows;

namespace Doozr.Common.Application.Desktop.Wpf
{
	[Log]
	public class WindowPlacementPersistor : IWindowPlacementPersistor, ILoggingObject
	{
		private readonly IApplicationDataStore applicationDataStore;
		private readonly IWindowPlacementManager windowPlacementManager;

		public WindowPlacementPersistor(IApplicationDataStore applicationDataStore, IWindowPlacementManager windowPlacementManager)
		{
			this.applicationDataStore = applicationDataStore;
			this.windowPlacementManager = windowPlacementManager;
		}

		public ILogger Logger { get; set; }

		public void Register(global::System.Windows.Window window)
		{
			try
			{
				windowPlacementManager.SetWindowPlacement(window, applicationDataStore.ReadFile("MainWindowPosition.json"));
			}
			catch
			{
				Logger?.LogWarning("Could not get window position (maybe file does not exist on first application run). Just show window with defaults.");
				window.WindowState = WindowState.Normal;
			}
			applicationDataStore.WriteFile("MainWindowPosition.json", windowPlacementManager.GetWindowPlacement(window));
			window.Closing += (sender, cancelEventArges) =>
			{
				Logger?.Log("MainWindow closing.");
				applicationDataStore.WriteFile("MainWindowPosition.json", windowPlacementManager.GetWindowPlacement(window));
			};
		}
	}
}
