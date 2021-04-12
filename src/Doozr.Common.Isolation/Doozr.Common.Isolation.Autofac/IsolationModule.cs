using Autofac;
using Doozr.Common.Isolation.Io;
using Doozr.Common.Isolation.Time;

namespace Doozr.Common.Isolation.Autofac
{
	public class IsolationModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>().SingleInstance();

			builder.RegisterType<FileSystem>().As<IFileSystem>();
		}
	}
}
