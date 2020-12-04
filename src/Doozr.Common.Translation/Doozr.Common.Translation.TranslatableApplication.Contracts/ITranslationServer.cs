namespace Doozr.Common.Translation.TranslatableApplication
{
	public interface ITranslationServer
	{
		ITranslatorApplication TranslatorApplication{ get; }

		void Start();

		void Stop();
	}
}
