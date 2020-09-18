using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Doozr.Common.Ipc
{
	public class CommandHandler
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

		private Dictionary<Guid, CommandContext> commandContexts = new Dictionary<Guid, CommandContext>();

		private Dictionary<Type, object> commandHandlers = new Dictionary<Type, object>();

		public CommandHandler(IMessageReceiver messageReceiver)
		{
			this.messageReceiver = messageReceiver;
			this.messageReceiver.OnMessageReceived += OnMessageReceived;
		}

		public void AddCommandContext(CommandContext context)
		{
			if (context.CommandId == Guid.Empty)
			{

			}
			commandContexts.Add(context.CommandId, context);
		}

		public void AddHandler<T>(T handler)
		{
			commandHandlers.Add(typeof(T), handler);
		}

		public T GetCommandProxy<T>() where T:class
		{
			object proxy = DispatchProxy.Create<T, CommandProxy>();
			((CommandProxy)proxy).MessageSender = (IMessageSender)this.messageReceiver; // please fix this
			((CommandProxy)proxy).AddCommandContext = AddCommandContext;
			return (T)proxy;
		}

		private void OnMessageReceived(byte[] messageBytes, Action<byte[]> sendReply)
		{
			var reader = new ByteArrayReader(messageBytes);

			var commandId = reader.ReadGuid();
			if (commandContexts.ContainsKey(commandId))
			{
				HandleResponse(commandId, reader);
			}
			else
			{
				HandleCommand(commandId, reader, sendReply);
			}
		}

		private void HandleCommand(Guid commandId, ByteArrayReader reader, Action<byte[]> sendReply)
		{
			string typeName = reader.ReadString();
			string methodName = reader.ReadString();

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
			
			var result = method.Invoke(handlerObject, paramCount == 0 ? null : parameters.ToArray());

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
