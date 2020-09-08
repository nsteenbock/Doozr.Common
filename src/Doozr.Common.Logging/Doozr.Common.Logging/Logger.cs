using System;

namespace Doozr.Common.Logging
{
	public abstract class Logger : ILogger
	{
		public abstract void EnterMethod(string methodName);
		public abstract void EnterMethod(LogLevel logLevel, string methodName);
		public abstract void EnterMethod(object instance, string methodName);
		public abstract void EnterMethod(LogLevel logLevel, object instance, string methodName);
		public abstract void LeaveMethod(LogLevel logLevel, string methodName);
		public abstract void LeaveMethod(string methodName);
		public abstract void LeaveMethod(object instance, string methodName);
		public abstract void LeaveMethod(LogLevel logLevel, object instance, string methodName);
		public abstract void Log(string message);
		public abstract void Log(LogLevel logLevel, string message);
		public abstract void LogException(Exception exception);
		public abstract void LogException(string message, Exception exception);
		public abstract void LogWarning(string message);
	}
}
