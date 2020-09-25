using System.Collections.Generic;
using System.Globalization;

namespace Doozr.Common.I18n
{
	public interface ITranslationProvider
	{
		CultureInfo[] GetAvailableCultures();

		Dictionary<string, Translation> GetTranslations(CultureInfo cultureInfo);
	}
}
