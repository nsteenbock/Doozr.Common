using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Doozr.Common.Application.Desktop.Wpf
{
	public class WindowPlacementPersistor : IWindowPlacementPersistor
	{
		private readonly IApplicationDataStore applicationDataStore;
		private readonly IWindowPlacementManager windowPlacementManager;

		public WindowPlacementPersistor(IApplicationDataStore applicationDataStore, IWindowPlacementManager windowPlacementManager)
		{
			this.applicationDataStore = applicationDataStore;
			this.windowPlacementManager = windowPlacementManager;
		}

		public void Register(global::System.Windows.Window window)
		{
			try
			{
				windowPlacementManager.SetWindowPlacement(window, applicationDataStore.ReadFile("MainWindowPosition.json"));
			}
			catch
			{
				window.WindowState = WindowState.Normal;
			}
			applicationDataStore.WriteFile("MainWindowPosition.json", windowPlacementManager.GetWindowPlacement(window));
			window.Closing += (sender, cancelEventArges) =>
			{
				applicationDataStore.WriteFile("MainWindowPosition.json", windowPlacementManager.GetWindowPlacement(window));
			};
		}
	}
}
