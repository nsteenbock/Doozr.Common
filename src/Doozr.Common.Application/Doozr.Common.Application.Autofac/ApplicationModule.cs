using Autofac;

namespace Doozr.Common.Application
{
	public class ApplicationModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ApplicationProperties>().As<IApplicationProperties>().SingleInstance();
			builder.RegisterType<ApplicationDataStore>().As<IApplicationDataStore>().SingleInstance();
			builder.RegisterType<ProcessNamePortableApplicationDetector>().As<IPortableApplicationDetector>().SingleInstance();
		}
	}
}
