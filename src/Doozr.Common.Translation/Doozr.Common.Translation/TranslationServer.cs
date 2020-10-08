using Doozr.Common.Ipc;
using System.Diagnostics;

namespace Doozr.Common.Translation
{
	public class TranslationServer: ITranslationServer
	{
		private readonly ITranslationTarget translationTarget;
		private NamedPipesMessageServer server;
		private CommandHandler commandHandler;
		private ITranslator translator;

		public TranslationServer(ITranslationTarget translationTarget)
		{
			this.translationTarget = translationTarget;
		}

		public void Start()
		{
			server = new NamedPipesMessageServer(string.Format(Consts.TRANSLATION_PIPE_NAME, Process.GetCurrentProcess().Id), 3, 1);
			server.Start();
			commandHandler = new CommandHandler(server);
			commandHandler.AddHandler<ITranslationTarget>(translationTarget);
			translator = commandHandler.GetCommandProxy<ITranslator>();
		}

		public void Stop()
		{
			server.Stop();
			server = null;
		}
	}
}
