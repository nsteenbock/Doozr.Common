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
		private readonly CommandHandler.Factory commandHandlerFactory;
		private readonly NamedPipesMessageServer.Factory namedPipesMessageServerFactory;
		private INamedPipesMessageServer server;
		private CommandHandler commandHandler;
		private ITranslator translator;

		public delegate ITranslationServer Factory(TranslationTarget translationTarget);

		public TranslationServer(ITranslationTarget translationTarget, CommandHandler.Factory commandHandlerFactory, NamedPipesMessageServer.Factory namedPipesMessageServerFactory)
		{
			this.translationTarget = translationTarget;
			this.commandHandlerFactory = commandHandlerFactory;
			this.namedPipesMessageServerFactory = namedPipesMessageServerFactory;
		}

		public ILogger Logger { get; set; }

		public void Start()
		{

			string pipeName = string.Format(Consts.TRANSLATION_PIPE_NAME, Process.GetCurrentProcess().Id);
			Logger?.LogString(nameof(pipeName), pipeName);

			server = namedPipesMessageServerFactory(pipeName, 3, 1);
			server.Start();
			commandHandler = commandHandlerFactory(server);
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
