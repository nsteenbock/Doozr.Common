namespace Doozr.Common.Translation
{
	public interface ITranslatableApplication
	{
		string GetNameOfSelectedCulture();

		string[] GetNamesOfAvailableCultures();

		void SelectCurrentCultureByName(string cultureName);

		I18n.Translation[] GetTranslations(string cultureName);

		void SetTranslation(string cultureName, I18n.Translation translation);

		void SaveTranslations(string cultureName, I18n.Translation[] translations);
	}
}
