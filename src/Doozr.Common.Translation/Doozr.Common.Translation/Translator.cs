using Doozr.Common.Ipc;
using System.Threading.Tasks;

namespace Doozr.Common.Translation
{
	public class Translator : ITranslator
	{
		private NamedPipeMessageClient client;
		private CommandHandler commandHandler;
		public ITranslationTarget TranslationTarget { get; private set; }

		public void Connect(string pipename)
		{
			if (client != null)
			{
				Task.Factory.StartNew(() => client.Disconnect()).Wait();
			}
			client = new NamedPipeMessageClient(".", pipename);
			client.Connect();
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
