using Doozr.Common.I18n;
using System.Globalization;
using System.Linq;

namespace Doozr.Common.Translation
{
	public class TranslationTarget: ITranslationTarget
	{
		private readonly ITranslationProvider translationProvider;
		private readonly ITranslationSource translationSource;

		public TranslationTarget(ITranslationProvider translationProvider, ITranslationSource translationSource)
		{
			this.translationProvider = translationProvider;
			this.translationSource = translationSource;
		}

		public string[] GetNamesOfAvailableCultures()
		{
			return translationProvider.GetAvailableCultures().Select(x => x.Name).ToArray();
		}

		public void SelectCulture(string cultureName)
		{
			translationSource.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
		}
	}
}
