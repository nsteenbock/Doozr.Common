namespace Doozr.Common.Translation
{
	public interface ITranslatorApplication
	{
		void ReportMissingTranslation(string cultureName, string key);

		void ReportLanguageChange(string cultureName);
	}
}
