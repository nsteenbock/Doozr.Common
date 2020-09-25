using System;
using System.ComponentModel;
using System.Globalization;

namespace Doozr.Common.I18n
{
	public interface ITranslationSource : INotifyPropertyChanged
	{
		CultureInfo[] AvailableCultures { get; }
		CultureInfo CurrentCulture { get; set; }

		event Action<string, string> OnMissingTranslation;

		void SetTranslation(string translationKey, Translation translation);
	}
}
