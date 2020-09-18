using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Doozr.Common.Ipc
{
	public class NamedPipesMessageServer: INamedPipesMessageServer
	{
		private readonly string pipeName;
		private readonly int maxNumberOfInstances;
		private readonly int initialNumberOfInstances;

		private CancellationTokenSource cancellationTokenSource;
		private int numberOfPipes;
		private int numberOfConnectedClients;

		private readonly List<NamedPipeServerStream> streams = new List<NamedPipeServerStream>();
		private readonly List<Task> startedTasks = new List<Task>();

		public NamedPipesMessageServer(
			string pipeName,
			int maxNumberOfInstances,
			int initialNumberOfInstances)
		{
			this.pipeName = pipeName;
			this.maxNumberOfInstances = maxNumberOfInstances;
			this.initialNumberOfInstances = initialNumberOfInstances;
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
		}

		private NamedPipeServerStream CreateNewServerStream()
		{
			return new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxNumberOfInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
		}

		public void Stop()
		{
			cancellationTokenSource.Cancel();

			Task.WaitAll(startedTasks.ToArray());
			
			foreach(var stream in streams)
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
				await pipeStream.WaitForConnectionAsync(cancellationToken);
				Interlocked.Increment(ref numberOfConnectedClients);
				CheckForPipeCreation();
				while (pipeStream.IsConnected && ! cancellationToken.IsCancellationRequested)
				{
					try
					{
						var bytesRead = await pipeStream.ReadAsync(buffer, 0, buffer.Length);
						if (bytesRead >= 0)
						{
							receivedBytes.AddRange(buffer.Take(bytesRead));
						}

						if (pipeStream.IsMessageComplete)
						{
							OnMessageReceived?.Invoke(receivedBytes.ToArray(), (bytes) => pipeStream.Write(bytes, 0, bytes.Length));
							receivedBytes.Clear();
						}
					}
					catch(Exception ex)
					{

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
			foreach(var stream in streams)
			{
				if (stream.IsConnected)
				{
					stream.Write(message, 0, message.Length);
				}
			}
		}
	}
}
