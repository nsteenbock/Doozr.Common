namespace Doozr.Common.Translation
{
	public interface ITranslatableApplication
	{
		string GetNameOfSelectedCulture();

		string[] GetNamesOfAvailableCultures();

		void SelectCurrentCultureByName(string cultureName);
	}
}
