using Autofac;

namespace Doozr.Common.Translation.Autofac
{
	public class TranslationModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<TranslationServer>().As<ITranslationServer>();
			builder.RegisterType<TranslationTarget>().As<ITranslationTarget>();
			builder.RegisterType<Translator>().As<ITranslator>();
		}
	}
}
