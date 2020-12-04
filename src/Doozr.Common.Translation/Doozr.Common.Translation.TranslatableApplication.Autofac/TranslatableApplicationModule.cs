using Autofac;

namespace Doozr.Common.Translation.TranslatableApplication.Autofac
{
	public class TranslatableApplicationModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<TranslationServer>().As<ITranslationServer>();
		}
	}
}
