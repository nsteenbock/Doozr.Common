namespace Doozr.Common.Translation
{
	public interface ITranslatorClientAccess
	{
		void ReportMissingTranslation(string cultureName, string key);
	}
}
