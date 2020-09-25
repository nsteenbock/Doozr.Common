namespace Doozr.Common.I18n.Translator
{
	public interface ITranslator
	{
		void ReportMissingTranslation(string translationKey, string cultureCode);

		void ReportLanguageChange(string cultureCode);
	}
}
