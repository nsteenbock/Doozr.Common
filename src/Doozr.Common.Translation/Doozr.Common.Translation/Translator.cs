using Doozr.Common.Ipc;
using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doozr.Common.Translation
{
	[Log]
	public class Translator : ITranslator, ILoggingObject
	{
		private INamedPipeMessageClient client;
		private CommandHandler commandHandler;
		private readonly NamedPipeMessageClient.Factory messageClientFactory;
		private readonly CommandHandler.Factory commandHandlerFactory;
		private readonly INamedPipeManager namedPipeManager;

		public ITranslationTarget TranslationTarget { get; private set; }
		public ILogger Logger { get; set; }

		public delegate ITranslator Factory();

		public Translator(NamedPipeMessageClient.Factory messageClientFactory, CommandHandler.Factory commandHandlerFactory, INamedPipeManager namedPipeManager)
		{
			this.messageClientFactory = messageClientFactory;
			this.commandHandlerFactory = commandHandlerFactory;
			this.namedPipeManager = namedPipeManager;
		}

		public void Connect(string pipename)
		{
			Logger?.LogString(nameof(pipename), pipename);

			if (client != null)
			{
				Logger?.Log("Disconnecting existing connection.");
				Task.Factory.StartNew(() => client.Disconnect()).Wait();
			}
			client = messageClientFactory(".", pipename);
			Logger?.Log("Connecting.");
			client.Connect();
			Logger?.Log("Connection established.");

			commandHandler = commandHandlerFactory(client);
			TranslationTarget = commandHandler.GetCommandProxy<ITranslationTarget>();
		}

		public void Disconnect()
		{
			client.Disconnect();
			client = null;
		}

		public TranslationServerProcessInfo[] GetAvailableTranslationServers()
		{
			var doozrTranslationPipes = namedPipeManager.GetNamedPipes()
				.Where(x => x.StartsWith(Consts.TRANSLATION_PIPE_PREFIX));
			var result = new List<TranslationServerProcessInfo>();

			foreach(var doozrTranslationPipe in doozrTranslationPipes)
			{
				var processIdString = doozrTranslationPipe.Remove(0, Consts.TRANSLATION_PIPE_PREFIX.Length);
				int processId;
				if (int.TryParse(processIdString, out processId))
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
	}
}
