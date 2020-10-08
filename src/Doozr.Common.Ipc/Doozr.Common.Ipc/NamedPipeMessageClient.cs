using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doozr.Common.Ipc
{
	public class NamedPipeMessageClient: INamedPipesMessageClient
	{
		private readonly string server;
		private readonly string pipeName;
		private NamedPipeClientStream pipeClient;
		private CancellationTokenSource cancellationTokenSource;
		private Task loopTask;

		public NamedPipeMessageClient(string server, string pipeName)
		{
			this.server = server;
			this.pipeName = pipeName;
		}

		public void Connect()
		{
			pipeClient = new NamedPipeClientStream(server, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
			pipeClient.Connect();
			pipeClient.ReadMode = PipeTransmissionMode.Message;
			cancellationTokenSource = new CancellationTokenSource();
			loopTask = ClientStreamLoop(cancellationTokenSource.Token);
		}

		private async Task ClientStreamLoop(CancellationToken cancellationToken)
		{
			List<byte> receivedBytes = new List<byte>();
			byte[] buffer = new byte[4096];

			while (pipeClient.IsConnected && !cancellationToken.IsCancellationRequested)
			{
				try
				{
					var bytesReceived = await pipeClient.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
					receivedBytes.AddRange(buffer.Take(bytesReceived));
					if (pipeClient.IsMessageComplete)
					{
						OnMessageReceived?.Invoke(receivedBytes.ToArray(), SendMessage);
						receivedBytes.Clear();
					}
				}
				catch(Exception ex)
				{

				}
			}
		}

		public void SendMessage(byte[] message)
		{
				

			pipeClient.Write(message, 0, message.Length);
			pipeClient.Flush();
		}

		public void Disconnect()
		{
			cancellationTokenSource.Cancel();
			try
			{
				loopTask.Wait();
			}
			catch { }
			pipeClient.Dispose();
		}

		public event Action<byte[], Action<byte[]>> OnMessageReceived;
	}
}
