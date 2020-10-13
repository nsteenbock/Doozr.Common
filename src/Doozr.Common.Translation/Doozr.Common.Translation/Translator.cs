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

		public ITranslationTarget TranslationTarget { get; private set; }
		public ILogger Logger { get; set; }

		public Translator(NamedPipeMessageClient.Factory messageClientFactory)
		{
			this.messageClientFactory = messageClientFactory;
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

			commandHandler = new CommandHandler(client);
			TranslationTarget = commandHandler.GetCommandProxy<ITranslationTarget>();
		}

		public void Disconnect()
		{
			client.Disconnect();
			client = null;
		}
	}
}
