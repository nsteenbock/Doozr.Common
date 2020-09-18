namespace Doozr.Common.Ipc
{
	public interface INamedPipesMessageClient: IMessageReceiver, IMessageSender
	{
		void Connect();
		void Disconnect();
	}
}
