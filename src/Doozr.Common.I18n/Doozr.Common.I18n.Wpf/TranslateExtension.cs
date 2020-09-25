using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Doozr.Common.I18n.Wpf
{
	public class TranslateExtension : MarkupExtension
	{
		private readonly string translationKey;

		public static ITranslationSource TranslationSource{ get; set; }

		public TranslateExtension(string translationKey)
		{
			this.translationKey = translationKey;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (TranslationSource == null)
			{
				return "!" + translationKey + "!";
			}
			else
			{
				var binding = new Binding(translationKey)
				{
					Source = TranslationSource
				};
				return binding.ProvideValue(serviceProvider);
			}
		}
	}
}
