﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace Doozr.Common.I18n
{
	public interface ITranslationSource : INotifyPropertyChanged
	{
		CultureInfo[] AvailableCultures { get; }
		CultureInfo CurrentCulture { get; set; }

		event EventHandler<MissingTranslationArgs> MissingTranslation;

		event EventHandler<LanguageChangedArgs> LanguageChanged;

		void SetTranslation(CultureInfo culture, Translation translation);
	}
}
