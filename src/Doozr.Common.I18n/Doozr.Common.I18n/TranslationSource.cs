using Doozr.Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;

namespace Doozr.Common.I18n
{
	public class TranslationSource: DynamicObject, ITranslationSource, ILoggingObject
	{
		private Dictionary<string, Translation> translations = new Dictionary<string, Translation>();
		private readonly ITranslationProvider translationProvider;

		
		public event PropertyChangedEventHandler PropertyChanged;

		public event EventHandler<MissingTranslationArgs> MissingTranslation;

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

		public ILogger Logger { get; set; }

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var key = binder.Name;
			if (translations.ContainsKey(key))
			{
				result = translations[key].Value;
				return true;
			}
			
			result = $"_{key}_";

			Logger.LogWarning($"Missing translation '{key}' for language '{currentCulture.Name}");
			MissingTranslation?.Invoke(this, new MissingTranslationArgs(currentCulture.Name, key));
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
