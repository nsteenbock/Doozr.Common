using System;

namespace Doozr.Common.Translation
{
	public interface ITranslationTarget
	{
		string[] GetNamesOfAvailableCultures();
		string GetSelectedCulture();
		void SelectCulture(string cultureName);

		event Action<string, string> OnMissingTranslation;
	}
}
