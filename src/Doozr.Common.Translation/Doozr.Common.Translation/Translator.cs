using Doozr.Common.Ipc;
using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System.Threading.Tasks;

namespace Doozr.Common.Translation
{
	[Log]
	public class Translator : ITranslator, ILoggingObject
	{
		private NamedPipeMessageClient client;
		private CommandHandler commandHandler;
		public ITranslationTarget TranslationTarget { get; private set; }
		public ILogger Logger { get; set; }

		public void Connect(string pipename)
		{
			Logger?.LogString(nameof(pipename), pipename);

			if (client != null)
			{
				Logger?.Log("Disconnecting existing connection.");
				Task.Factory.StartNew(() => client.Disconnect()).Wait();
			}
			client = new NamedPipeMessageClient(".", pipename);
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
