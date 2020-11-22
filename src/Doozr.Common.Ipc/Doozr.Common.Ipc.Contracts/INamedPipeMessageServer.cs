namespace Doozr.Common.Ipc
{
	public interface INamedPipeMessageServer: IMessageReceiver, IMessageSender
	{
		void Start();

		void Stop();
	}
}
