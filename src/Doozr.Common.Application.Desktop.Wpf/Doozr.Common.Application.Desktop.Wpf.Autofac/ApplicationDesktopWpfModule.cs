using Autofac;

namespace Doozr.Common.Application.Desktop.Wpf.Autofac
{
	public class ApplicationDesktopWpfModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<WindowPlacementManager>().As<IWindowPlacementManager>().SingleInstance();
			builder.RegisterType<WindowPlacementPersistor>().As<IWindowPlacementPersistor>().SingleInstance();
		}
	}
}
