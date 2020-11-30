namespace Doozr.Common.Translation
{
	public interface ITranslationServer
	{
		ITranslatorClientAccess Translator { get; }

		void Start();
		void Stop();
	}
}
