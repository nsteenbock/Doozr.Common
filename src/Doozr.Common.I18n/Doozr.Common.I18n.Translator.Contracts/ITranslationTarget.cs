namespace Doozr.Common.I18n.Translator
{
	public interface ITranslationTarget
	{
		string[] GetAvailableCultures();

		string GetCurrentCulture();

		void UpdateTranslation(string translationKey, string cultureCode, string translation);

		void ChangeLanguage(string cultureCode);

		void RefreshTranslations();
	}
}
