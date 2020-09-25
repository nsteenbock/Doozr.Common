using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;

namespace Doozr.Common.I18n
{
	public class TranslationSource: DynamicObject, ITranslationSource
	{
		private Dictionary<string, Translation> translations = new Dictionary<string, Translation>();
		private readonly ITranslationProvider translationProvider;

		public event Action<string, string> OnMissingTranslation;
		public event PropertyChangedEventHandler PropertyChanged;

		public TranslationSource(ITranslationProvider translationProvider)
		{
			this.translationProvider = translationProvider;
			AvailableCultures = translationProvider.GetAvailableCultures();
		}

		public CultureInfo[] AvailableCultures { get; }

		private CultureInfo currentCulture;

		public CultureInfo CurrentCulture
		{
			get{ return currentCulture; }
			set
			{
				if (currentCulture != value)
				{
					currentCulture = value;
					translations = translationProvider.GetTranslations(currentCulture);
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
				}
			}
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var key = binder.Name;
			if (translations.ContainsKey(key))
			{
				result = translations[key].Value;
				return true;
			}
			
			result = $"_{key}_";
			OnMissingTranslation?.Invoke(key, currentCulture.Name);
			return true;
		}

		public void SetTranslation(string translationKey, Translation translation)
		{
			if (translations.ContainsKey(translationKey))
			{
				translations.Remove(translationKey);
			}

			translations.Add(translationKey, translation);

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(translationKey));
		}
	}
}
