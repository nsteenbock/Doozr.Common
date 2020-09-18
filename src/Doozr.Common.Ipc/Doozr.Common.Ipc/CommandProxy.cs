using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Doozr.Common.Ipc
{
	public class CommandProxy: DispatchProxy
	{
		public IMessageSender MessageSender { get; set; }

		public Action<CommandHandler.CommandContext> AddCommandContext{ get; set; }

		protected override object Invoke(MethodInfo targetMethod, object[] args)
		{
			var messageId = Guid.NewGuid();
			var writer = new ByteArrayWriter();
			writer.WriteGuid(messageId);
			writer.WriteString(targetMethod.DeclaringType.FullName);
			writer.WriteString(targetMethod.Name);
			
			foreach(var param in args)
			{
				var ms = new MemoryStream();
				var bf = new BinaryFormatter();
				bf.Serialize(ms, param);
				writer.WriteBytes(ms.ToArray());	
			}
			
			MessageSender.SendMessage(writer.Bytes);
			var commandContext = new CommandHandler.CommandContext(messageId);

			if (targetMethod.ReturnType.FullName != "System.Void")
			{
				AddCommandContext(commandContext);
				commandContext.ResetEvent.WaitOne();

				return commandContext.Result;
			}
			else
				return null;
		}
	}
}
