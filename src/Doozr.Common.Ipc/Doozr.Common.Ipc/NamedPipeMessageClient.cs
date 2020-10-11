using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doozr.Common.Ipc
{
	[Log]
	public class NamedPipeMessageClient: INamedPipesMessageClient, ILoggingObject
	{
		private readonly string server;
		private readonly string pipeName;
		private NamedPipeClientStream pipeClient;
		private CancellationTokenSource cancellationTokenSource;
		private Task loopTask;

		public ILogger Logger { get; set; }

		public NamedPipeMessageClient(string server, string pipeName)
		{
			#region Instrumentation

			Logger?.EnterMethod("ctor");
			Logger?.LogString(nameof(server), server);
			Logger?.LogString(nameof(pipeName), pipeName);

			#endregion

			this.server = server;
			this.pipeName = pipeName;

			#region Instrumentation

			Logger?.LeaveMethod("ctor");

			#endregion
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
					Logger?.Log("Waiting for bytes received");

					var bytesReceived = await pipeClient.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

					Logger?.LogInt(nameof(bytesReceived), bytesReceived);
					
					receivedBytes.AddRange(buffer.Take(bytesReceived));
					if (pipeClient.IsMessageComplete)
					{
						Logger?.Log("MessageComplete");

						OnMessageReceived?.Invoke(receivedBytes.ToArray(), SendMessage);
						receivedBytes.Clear();
					}
				}
				catch(Exception ex)
				{
					Logger?.LogException(ex);
				}
			}
		}

		public void SendMessage(byte[] message)
		{
			#region Instrumentation

			Logger?.LogInt("messageLength", message.Length);
			
			#endregion

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
			catch (Exception ex)
			{
				Logger?.LogException(ex);
			}
			pipeClient.Dispose();
		}

		public event Action<byte[], Action<byte[]>> OnMessageReceived;
	}
}
