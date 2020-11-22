using Autofac;

namespace Doozr.Common.Ipc.Autofac
{
	public class IpcModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<NamedPipeMessageClient>().As<INamedPipeMessageClient>();
			builder.RegisterType<NamedPipeMessageServer>().As<INamedPipeMessageServer>();
			builder.RegisterType<CommandHandler>();
			builder.RegisterType<NamedPipeManager>().As<INamedPipeManager>();
		}
	}
}
