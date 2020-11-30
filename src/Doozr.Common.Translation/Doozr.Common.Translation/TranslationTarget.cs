using Doozr.Common.I18n;
using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System;
using System.Globalization;
using System.Linq;

namespace Doozr.Common.Translation
{
	[Log]
	public class TranslationTarget: ITranslationTarget, ILoggingObject
	{
		private readonly ITranslationProvider translationProvider;
		private readonly ITranslationSource translationSource;

		public TranslationTarget(ITranslationProvider translationProvider, ITranslationSource translationSource)
		{
			this.translationProvider = translationProvider;
			this.translationSource = translationSource;

			translationSource.OnMissingTranslation += TranslationSource_OnMissingTranslation;
		}

		private void TranslationSource_OnMissingTranslation(string key, string cultureName)
		{
			OnMissingTranslation?.Invoke(key, cultureName);
		}

		public event Action<string, string> OnMissingTranslation;

		public ILogger Logger { get; set; }

		public string[] GetNamesOfAvailableCultures()
		{
			return translationProvider.GetAvailableCultures().Select(x => x.Name).ToArray();
		}

		public void SelectCulture(string cultureName)
		{
			Logger?.LogString(nameof(cultureName), cultureName);
			translationSource.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
		}

		public string GetSelectedCulture()
		{
			var nameOfCurrentlySelectedCulture = translationSource.CurrentCulture.Name;
			Logger?.LogString(nameof(nameOfCurrentlySelectedCulture), nameOfCurrentlySelectedCulture);
			return nameOfCurrentlySelectedCulture;
		}
	}
}
