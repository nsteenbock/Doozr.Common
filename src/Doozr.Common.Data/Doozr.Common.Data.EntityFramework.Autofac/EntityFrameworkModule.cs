using Autofac;

namespace Doozr.Common.Data.EntityFramework.Autofac
{
	public class EntityFrameworkModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<AuditFieldHandler>().As<IAuditFieldHandler>();
		}
	}
}
