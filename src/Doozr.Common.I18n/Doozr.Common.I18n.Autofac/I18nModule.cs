using Autofac;
using Doozr.Common.Application;
using System.IO;

namespace Doozr.Common.I18n.Autofac
{
	public class I18nModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c =>
				{
					var applicationProperties = c.Resolve<IApplicationProperties>();
					return new TranslationProvider(Path.Combine(applicationProperties.RootDirectory, "language"));
				}).As<ITranslationProvider>().SingleInstance();
			builder.RegisterType<TranslationSource>().As<ITranslationSource>();
		}
	}
}
