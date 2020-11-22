namespace Doozr.Common.Ipc
{
	public interface INamedPipeMessageClient: IMessageReceiver, IMessageSender
	{
		void Connect();
		void Disconnect();
	}
}
