namespace Doozr.Common.Translation
{
	public interface ITranslator
	{
		ITranslationTarget TranslationTarget { get; }

		void Connect(string pipename);
		void Disconnect();
	}
}
