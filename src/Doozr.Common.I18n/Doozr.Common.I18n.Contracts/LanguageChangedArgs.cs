namespace Doozr.Common.I18n
{
	public class LanguageChangedArgs
	{
		public LanguageChangedArgs(string cultureName)
		{
			CultureName = cultureName;
		}

		public string CultureName { get; }
	}
}
