using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Doozr.Common.Ipc
{
	[Log]
	public class CommandHandler: ILoggingObject
	{
		public class CommandContext
		{
			public ManualResetEvent ResetEvent { get; }

			public CommandContext(Guid commandId)
			{
				CommandId = commandId;
				ResetEvent = new ManualResetEvent(false);
			}

			public Guid CommandId{ get; }

			public object Result{ get; set; }
		}

		private readonly IMessageReceiver messageReceiver;

		private readonly Dictionary<Guid, CommandContext> commandContexts = new Dictionary<Guid, CommandContext>();

		private readonly Dictionary<Type, object> commandHandlers = new Dictionary<Type, object>();

		public ILogger Logger { get; set; }

		public CommandHandler(IMessageReceiver messageReceiver)
		{
			this.messageReceiver = messageReceiver;
			this.messageReceiver.OnMessageReceived += OnMessageReceived;
		}

		public void AddCommandContext(CommandContext context)
		{
			Logger?.LogString("commandId", context.CommandId.ToString());
			if (context.CommandId == Guid.Empty)
			{

			}
			else
				commandContexts.Add(context.CommandId, context);
		}

		public void AddHandler<T>(T handler)
		{
			Logger?.LogString("HandlerType", typeof(T).ToString());
			commandHandlers.Add(typeof(T), handler);
		}

		public T GetCommandProxy<T>() where T:class
		{
			Logger.LogString("ProxyFor", typeof(T).ToString());
			object proxy = DispatchProxy.Create<T, CommandProxy>();
			((CommandProxy)proxy).MessageSender = (IMessageSender)this.messageReceiver; // please fix this
			((CommandProxy)proxy).AddCommandContext = AddCommandContext;
			return (T)proxy;
		}

		private void OnMessageReceived(byte[] messageBytes, Action<byte[]> sendReply)
		{
			Logger?.LogInt("messageLength", messageBytes.Length);
			var reader = new ByteArrayReader(messageBytes);

			var commandId = reader.ReadGuid();

			Logger?.LogString(nameof(commandId), commandId.ToString());

			if (commandContexts.ContainsKey(commandId))
			{
				Logger?.Log("Handle command with return value.");
				HandleResponse(commandId, reader);
			}
			else
			{
				Logger?.Log("No command context found. => Handle command without return value.");
				HandleCommand(commandId, reader, sendReply);
			}
		}

		private void HandleCommand(Guid commandId, ByteArrayReader reader, Action<byte[]> sendReply)
		{
			string typeName = reader.ReadString();
			string methodName = reader.ReadString();

			#region Instrumentation

			Logger?.LogString(nameof(typeName), typeName);
			Logger?.LogString(nameof(methodName), methodName);

			#endregion

			var handlerType = commandHandlers.Keys.Where(x => x.FullName == typeName).Single();
			var handlerObject = commandHandlers[handlerType];

			var method = handlerObject.GetType().GetMethod(methodName);

			var paramCount = method.GetParameters().Length;

			BinaryFormatter bf = new BinaryFormatter();

			List<object> parameters = new List<object>();

			for(int i = 0; i < paramCount; i++)
			{
				var paramBytes = reader.ReadBytes();
				parameters.Add(bf.Deserialize(new MemoryStream(paramBytes)));
			}

			Logger?.Log("Invoke command");

			var result = method.Invoke(handlerObject, paramCount == 0 ? null : parameters.ToArray());

			Logger?.Log("Finished invoking command");

			var writer = new ByteArrayWriter();
			writer.WriteGuid(commandId);

			if (method.ReturnType.FullName != "System.Void")
			{
				using (var ms = new MemoryStream())
				{
					bf.Serialize(ms, result);
					var bytes = ms.ToArray();
					writer.WriteBytes(bytes);
				}

				sendReply(writer.Bytes);
			}
		}

		private void HandleResponse(Guid commandId, ByteArrayReader reader)
		{
			Logger?.LogString(nameof(commandId), commandId.ToString());

			var context = commandContexts[commandId];
			var bytes = reader.ReadBytes();

			var bf = new BinaryFormatter();

			
			context.Result = bf.Deserialize(new MemoryStream(bytes));
			context.ResetEvent.Set(); 
			commandContexts.Remove(commandId);
			//todo: dipose context
		}
	}
}
