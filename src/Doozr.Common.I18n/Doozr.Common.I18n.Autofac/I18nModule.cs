using Autofac;

namespace Doozr.Common.I18n.Autofac
{
	public class I18nModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<TranslationProvider>().As<ITranslationProvider>();
			builder.RegisterType<TranslationSource>().As<ITranslationSource>();
		}
	}
}
