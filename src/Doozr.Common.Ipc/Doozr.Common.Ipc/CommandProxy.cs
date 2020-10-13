using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Doozr.Common.Ipc
{
	[Log]
	public class CommandProxy: DispatchProxy, ILoggingObject
	{
		public IMessageSender MessageSender { get; set; }

		public Action<CommandHandler.CommandContext> AddCommandContext{ get; set; }
		public ILogger Logger { get; set; }

		protected override object Invoke(MethodInfo targetMethod, object[] args)
		{
			var messageId = Guid.NewGuid();
			var writer = new ByteArrayWriter();
			writer.WriteGuid(messageId);
			string targetClassName = targetMethod.DeclaringType.FullName;
			string targetMethodName = targetMethod.Name;

			#region Instrumentation

			Logger?.LogString(nameof(messageId), messageId.ToString());
			Logger?.LogString(nameof(targetClassName), targetClassName);
			Logger?.LogString(nameof(targetMethodName), targetMethodName);
				
			#endregion

			writer.WriteString(targetClassName);
			writer.WriteString(targetMethodName);
			
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

				Logger?.Log("Waiting for result");

				commandContext.ResetEvent.WaitOne();

				Logger?.Log("Result received");

				return commandContext.Result;
			}
			else
				return null;
		}
	}
}
