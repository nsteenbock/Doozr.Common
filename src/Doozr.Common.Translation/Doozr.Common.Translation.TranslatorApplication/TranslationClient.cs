using Doozr.Common.I18n;
using Doozr.Common.Ipc;
using Doozr.Common.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doozr.Common.Translation.TranslatorApplication
{
	public class TranslationClient : ITranslationClient, ITranslatorApplication, ILoggingObject
	{
		public ILogger Logger { get; set; }

		public ITranslatableApplication TranslatableApplication { get; private set; }

		private INamedPipeMessageClient namedPipeMessageClient;

		private CommandHandler commandHandler;

		private readonly NamedPipeMessageClient.Factory messageClientFactory;
		private readonly CommandHandler.Factory commandHandlerFactory;
		private readonly INamedPipeManager namedPipeManager;

		public event System.EventHandler<MissingTranslationArgs> MissingTranslation;

		public event System.EventHandler<LanguageChangedArgs> LanguageChanged;

		public TranslationClient(NamedPipeMessageClient.Factory messageClientFactory, CommandHandler.Factory commandHandlerFactory, INamedPipeManager namedPipeManager)
		{
			this.messageClientFactory = messageClientFactory;
			this.commandHandlerFactory = commandHandlerFactory;
			this.namedPipeManager = namedPipeManager;
		}

		public void Connect(string pipename)
		{
			Logger?.LogString(nameof(pipename), pipename);

			if (namedPipeMessageClient != null)
			{
				Logger?.Log("Disconnecting existing connection.");
				Task.Factory.StartNew(() => namedPipeMessageClient.Disconnect()).Wait();
			}
			namedPipeMessageClient = messageClientFactory(".", pipename);
			Logger?.Log("Connecting.");
			namedPipeMessageClient.Connect();
			Logger?.Log("Connection established.");

			commandHandler = commandHandlerFactory(namedPipeMessageClient);
			commandHandler.AddHandler<ITranslatorApplication>(this);
			TranslatableApplication = commandHandler.GetCommandProxy<ITranslatableApplication>();
		}

		public void Disconnect()
		{
			namedPipeMessageClient.Disconnect();
			namedPipeMessageClient = null;
		}

		public TranslationServerProcessInfo[] GetAvailableTranslationServers()
		{
			var doozrTranslationPipes = namedPipeManager.GetNamedPipes()
				.Where(x => x.StartsWith(Constants.TRANSLATION_PIPE_PREFIX));
			var result = new List<TranslationServerProcessInfo>();

			foreach (var doozrTranslationPipe in doozrTranslationPipes)
			{
				var processIdString = doozrTranslationPipe.Remove(0, Constants.TRANSLATION_PIPE_PREFIX.Length);
				if (int.TryParse(processIdString, out int processId))
				{
					result.Add(new TranslationServerProcessInfo
					{
						PipeName = doozrTranslationPipe,
						ProcessId = processId
					});
				}
			}

			return result.ToArray();
		}

		public void ReportMissingTranslation(string cultureName, string key)
		{
			MissingTranslation?.Invoke(this, new MissingTranslationArgs(cultureName, key));
		}

		public void ReportLanguageChange(string cultureName)
		{
			LanguageChanged?.Invoke(this, new LanguageChangedArgs(cultureName));
		}
	}
}
