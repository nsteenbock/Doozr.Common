using Doozr.Common.Ipc;
using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System.Threading.Tasks;

namespace Doozr.Common.Translation
{
	[Log]
	public class Translator : ITranslator, ILoggingObject
	{
		private INamedPipesMessageClient client;
		private CommandHandler commandHandler;
		private readonly NamedPipeMessageClient.Factory messageClientFactory;
		private readonly CommandHandler.Factory commandHandlerFactory;

		public ITranslationTarget TranslationTarget { get; private set; }
		public ILogger Logger { get; set; }

		public delegate ITranslator Factory();

		public Translator(NamedPipeMessageClient.Factory messageClientFactory, CommandHandler.Factory commandHandlerFactory)
		{
			this.messageClientFactory = messageClientFactory;
			this.commandHandlerFactory = commandHandlerFactory;
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
	}
}
