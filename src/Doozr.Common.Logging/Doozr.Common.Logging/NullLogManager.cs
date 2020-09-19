using System;

namespace Doozr.Common.Logging
{
	public class NullLogManager : LogManager
	{
		static ILogger loggerInstance = new NullLogger();

		public override ILogger GetLogger(Type type)
		{
			return loggerInstance;
		}

		class NullLogger : Logger
		{
			public override void EnterMethod(string methodName) { }

			public override void EnterMethod(LogLevel logLevel, string methodName) { }


			public override void EnterMethod(object instance, string methodName) { }

			public override void EnterMethod(LogLevel logLevel, object instance, string methodName) { }

			public override void LeaveMethod(LogLevel logLevel, string methodName) { }

			public override void LeaveMethod(string methodName) { }

			public override void LeaveMethod(object instance, string methodName) { }

			public override void LeaveMethod(LogLevel logLevel, object instance, string methodName) { }

			public override void Log(string message) { }

			public override void Log(LogLevel logLevel, string message) { }

			public override void LogException(Exception exception) { }

			public override void LogException(string message, Exception exception) { }

			public override void LogWarning(string message) { }
		}
	}
}