namespace Doozr.Common.I18n
{
	public class MissingTranslationArgs
	{
		public MissingTranslationArgs(string cultureName, string key)
		{
			CultureName = cultureName;
			Key = key;
		}

		public string CultureName { get; }
		public string Key { get; }
	}
}
