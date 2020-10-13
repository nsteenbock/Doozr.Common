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
	public class NamedPipesMessageServer: INamedPipesMessageServer, ILoggingObject
	{
		private readonly string pipeName;
		private readonly int maxNumberOfInstances;
		private readonly int initialNumberOfInstances;

		private CancellationTokenSource cancellationTokenSource;
		private int numberOfPipes;
		private int numberOfConnectedClients;

		private readonly List<NamedPipeServerStream> streams = new List<NamedPipeServerStream>();
		private readonly List<Task> startedTasks = new List<Task>();

		public ILogger Logger { get; set; }

		public delegate INamedPipesMessageServer Factory(string pipeName, int maxNumberOfInstances, int initialNumberOfInstances);

		public NamedPipesMessageServer(
			string pipeName,
			int maxNumberOfInstances,
			int initialNumberOfInstances)
		{
			#region Instrumentation

			Logger?.EnterMethod("ctor");
			Logger?.LogString(nameof(pipeName), pipeName);
			Logger?.LogInt(nameof(maxNumberOfInstances), maxNumberOfInstances);
			Logger?.LogInt(nameof(initialNumberOfInstances), initialNumberOfInstances);

			#endregion

			this.pipeName = pipeName;
			this.maxNumberOfInstances = maxNumberOfInstances;
			this.initialNumberOfInstances = initialNumberOfInstances;

			#region Instrumentation

			Logger?.LeaveMethod("ctor");

			#endregion
		}

		public event Action<byte[], Action<byte[]>> OnMessageReceived;

		public void Start()
		{
			if (streams.Count > 0) throw new InvalidOperationException("There are already active server streams.");

			cancellationTokenSource = new CancellationTokenSource();
			
			CreateServerStreams();
			StartServerStreams();
		}

		private void StartServerStreams()
		{	
			
			foreach (var stream in streams)
			{
				startedTasks.Add(ServerStreamLoop(stream, cancellationTokenSource.Token));
			}
		}

		private void CreateServerStreams()
		{
			for (int i = 0; i < initialNumberOfInstances; i++)
			{
				streams.Add(CreateNewServerStream());
			}

			#region Instrumentation

			Logger?.LogInt("numberOfServerStreams", streams.Count);
			
			#endregion
		}

		private NamedPipeServerStream CreateNewServerStream()
		{
			return new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxNumberOfInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
		}

		public void Stop()
		{
			foreach(var stream in streams)
			{
				if (stream.IsConnected)
				{
					stream.Disconnect();
				}
			}

			cancellationTokenSource.Cancel();

			try
			{
				Logger?.Log("Waiting for streams to shut down");
				Task.WaitAll(startedTasks.ToArray());
			}
			catch(Exception ex)
			{
				Logger?.LogException(ex);
			}

			Logger?.Log("Disposing streams");
			foreach (var stream in streams)
			{
				stream.Dispose();
			}

			cancellationTokenSource.Dispose();
			streams.Clear();
			startedTasks.Clear();
			numberOfPipes = 0;
			numberOfConnectedClients = 0;
		}

		private async Task ServerStreamLoop(NamedPipeServerStream pipeStream, CancellationToken cancellationToken)
		{
			List<byte> receivedBytes = new List<byte>();
			byte[] buffer = new byte[4096];

			Interlocked.Increment(ref numberOfPipes);
			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					Logger?.Log("Waiting for connection");
					
					await pipeStream.WaitForConnectionAsync(cancellationToken);

					Logger?.Log("Connected");
				}
				catch (Exception ex)
				{
					Logger?.LogException(ex);
				}

				Interlocked.Increment(ref numberOfConnectedClients);
				CheckForPipeCreation();
				while (pipeStream.IsConnected && ! cancellationToken.IsCancellationRequested)
				{
					try
					{
						Logger?.Log("Waiting for bytes to read");
						var bytesRead = await pipeStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
						Logger?.Log("Bytes received");
						Logger?.LogInt(nameof(bytesRead), bytesRead);
						if (bytesRead >= 0)
						{
							receivedBytes.AddRange(buffer.Take(bytesRead));
						}

						if (pipeStream.IsMessageComplete)
						{
							Logger?.Log("Message complete");
							if (receivedBytes.Count > 0)
							{
								OnMessageReceived?.Invoke(receivedBytes.ToArray(), (bytes) =>
									{
										pipeStream.Write(bytes, 0, bytes.Length);
										pipeStream.Flush();
									});
								receivedBytes.Clear();
							}
						}
					}
					catch(Exception ex)
					{
						Logger?.LogException(ex);
					}
				}
				pipeStream.Disconnect();
				Interlocked.Decrement(ref numberOfConnectedClients);
			}
			Interlocked.Decrement(ref numberOfPipes);
		}

		private void CheckForPipeCreation()
		{
			if (numberOfConnectedClients == numberOfPipes && numberOfPipes < maxNumberOfInstances)
			{
				var pipe = CreateNewServerStream();
				streams.Add(pipe);
				startedTasks.Add(ServerStreamLoop(pipe, cancellationTokenSource.Token));
			}
		}

		public void SendMessage(byte[] message)
		{
			#region Instrumentation

			Logger?.LogInt("messageLength", message.Length);

			#endregion

			foreach (var stream in streams)
			{
				if (stream.IsConnected)
				{
					stream.Write(message, 0, message.Length);
				}
			}
		}
	}
}
