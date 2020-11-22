using Autofac;

namespace Doozr.Common.Ipc.Autofac
{
	public class IpcModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<NamedPipeMessageClient>().As<INamedPipesMessageClient>();
			builder.RegisterType<NamedPipesMessageServer>().As<INamedPipesMessageServer>();
			builder.RegisterType<CommandHandler>();
			builder.RegisterType<NamedPipeManager>().As<INamedPipeManager>();
		}
	}
}
