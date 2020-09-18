namespace Doozr.Common.Ipc
{
	public interface INamedPipesMessageServer: IMessageReceiver, IMessageSender
	{
		void Start();

		void Stop();
	}
}
