using Doozr.Common.I18n;
using System;

namespace Doozr.Common.Translation.TranslatorApplication
{
	public interface ITranslationClient
	{
		ITranslatableApplication TranslatableApplication { get; }

		void Connect(string pipename);

		void Disconnect();

		event EventHandler<MissingTranslationArgs> MissingTranslation;

		TranslationServerProcessInfo[] GetAvailableTranslationServers();
	}
}
