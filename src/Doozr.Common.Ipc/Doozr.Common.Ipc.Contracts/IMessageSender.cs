namespace Doozr.Common.Ipc
{
	public interface IMessageSender
	{
		void SendMessage(byte[] message);
	}
}
