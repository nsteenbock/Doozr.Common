using Doozr.Common.Ipc;
using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System.Diagnostics;

namespace Doozr.Common.Translation
{
	[Log]
	public class TranslationServer: ITranslationServer, ILoggingObject
	{
		private readonly ITranslationTarget translationTarget;
		private NamedPipesMessageServer server;
		private CommandHandler commandHandler;
		private ITranslator translator;

		public TranslationServer(ITranslationTarget translationTarget)
		{
			this.translationTarget = translationTarget;
		}

		public ILogger Logger { get; set; }

		public void Start()
		{

			string pipeName = string.Format(Consts.TRANSLATION_PIPE_NAME, Process.GetCurrentProcess().Id);
			Logger?.LogString(nameof(pipeName), pipeName);

			server = new NamedPipesMessageServer(pipeName, 3, 1);
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
