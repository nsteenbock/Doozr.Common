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
		private readonly NamedPipeMessageServer.Factory namedPipeMessageServerFactory;
		private INamedPipeMessageServer server;
		private CommandHandler commandHandler;
		private ITranslator translator;

		public delegate ITranslationServer Factory(TranslationTarget translationTarget);

		public TranslationServer(ITranslationTarget translationTarget, CommandHandler.Factory commandHandlerFactory, NamedPipeMessageServer.Factory namedPipesMessageServerFactory)
		{
			this.translationTarget = translationTarget;
			this.commandHandlerFactory = commandHandlerFactory;
			this.namedPipeMessageServerFactory = namedPipesMessageServerFactory;
		}

		public ILogger Logger { get; set; }

		public void Start()
		{

			string pipeName = Consts.TRANSLATION_PIPE_PREFIX + Process.GetCurrentProcess().Id;
			Logger?.LogString(nameof(pipeName), pipeName);

			server = namedPipeMessageServerFactory(pipeName, 3, 1);
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
