﻿using Doozr.Common.I18n;
using Doozr.Common.Ipc;
using Doozr.Common.Logging;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Doozr.Common.Translation.TranslatableApplication
{
	public class TranslationServer : ITranslationServer, ITranslatableApplication, ILoggingObject
	{
		public ILogger Logger { get; set; }

		private readonly CommandHandler.Factory commandHandlerFactory;
		private readonly NamedPipeMessageServer.Factory namedPipeMessageServerFactory;
		private readonly ITranslationProvider translationProvider;
		private readonly ITranslationSource translationSource;
		private INamedPipeMessageServer namedPipeMessageServer;
		private CommandHandler commandHandler;
		private ITranslatorApplication translatorApplication;

		public TranslationServer(CommandHandler.Factory commandHandlerFactory, NamedPipeMessageServer.Factory namedPipeMessageServerFactory,
			ITranslationProvider translationProvider, ITranslationSource translationSource)
		{
			this.commandHandlerFactory = commandHandlerFactory;
			this.namedPipeMessageServerFactory = namedPipeMessageServerFactory;
			this.translationProvider = translationProvider;
			this.translationSource = translationSource;
			this.translationSource.MissingTranslation += OnMissingTranslation;
			this.translationSource.LanguageChanged += OnLanguageChanged;
		}

		private void OnMissingTranslation(object sender, MissingTranslationArgs e)
		{
			translatorApplication?.ReportMissingTranslation(e.CultureName, e.Key);
		}

		private void OnLanguageChanged(object sender, LanguageChangedArgs e)
		{
			translatorApplication?.ReportLanguageChange(e.CultureName);
		}

		public ITranslatorApplication TranslatorApplication
		{
			get { return translatorApplication; }
		}

		public void Start()
		{
			string pipeName = Constants.TRANSLATION_PIPE_PREFIX + Process.GetCurrentProcess().Id;
			Logger?.LogString(nameof(pipeName), pipeName);

			namedPipeMessageServer = namedPipeMessageServerFactory(pipeName, 3, 1);
			namedPipeMessageServer.Start();
			commandHandler = commandHandlerFactory(namedPipeMessageServer);
			commandHandler.AddHandler<ITranslatableApplication>(this);
			translatorApplication = commandHandler.GetCommandProxy<ITranslatorApplication>();
		}

		public void Stop()
		{
			namedPipeMessageServer.Stop();
			namedPipeMessageServer = null;
		}

		public string[] GetNamesOfAvailableCultures()
		{
			return translationProvider.GetAvailableCultures()
				.Select(x => x.Name).ToArray();
		}

		public void SelectCurrentCultureByName(string cultureName)
		{
			translationSource.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
		}

		public string GetNameOfSelectedCulture()
		{
			return translationSource.CurrentCulture.Name;
		}

		public I18n.Translation[] GetTranslations(string cultureName)
		{
			return translationProvider.GetTranslations(CultureInfo.GetCultureInfo(cultureName))
				.Values.ToArray();
		}

		public void SetTranslation(string cultureName, I18n.Translation translation)
		{
			translationSource.SetTranslation(CultureInfo.GetCultureInfo(cultureName), translation);
		}

		public void SaveTranslations(string cultureName, I18n.Translation[] translations)
		{
			translationProvider.SaveTranslations(CultureInfo.GetCultureInfo(cultureName), translations);
		}

		public void SetTranslations(string cultureName, I18n.Translation[] translations)
		{
			foreach(var translation in translations)
			{
				SetTranslation(cultureName, translation);
			}
		}
	}
}
