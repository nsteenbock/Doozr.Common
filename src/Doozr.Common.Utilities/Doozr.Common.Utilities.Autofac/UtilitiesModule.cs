using Autofac;
using Doozr.Common.Utilities.Filesystem;

namespace Doozr.Common.Utilities.Autofac
{
	public class UtilitiesModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<FileSearcher>().As<IFileSearcher>();
		}
	}
}
