using Autofac;
using Doozr.Common.Translation.TranslatorApplication;

namespace Doozr.Common.Translation.TranslatorApplication.Autofac
{
	public class TranslatorApplicationModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<TranslationClient>().As<ITranslationClient>().SingleInstance();
		}
	}
}
