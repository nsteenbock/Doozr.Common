namespace Doozr.Common.Translation
{
	public interface ITranslationTarget
	{
		string[] GetNamesOfAvailableCultures();

		void SelectCulture(string cultureName);
	}
}
