using System;

namespace Doozr.Common.Translation
{
	public interface ITranslator: ITranslatorClientAccess
	{
		ITranslationTarget TranslationTarget { get; }

		void Connect(string pipename);
		
		void Disconnect();
		
		TranslationServerProcessInfo[] GetAvailableTranslationServers();

		event Action<object, MissingTranslationEventArgs> OnMissingTranslation;
	}
}
