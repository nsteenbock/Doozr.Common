using System;

namespace Doozr.Common.Logging
{
	public interface ILogger
	{
		void Log(string message);
		void Log(LogLevel logLevel, string message);

		void EnterMethod(string methodName);
		void EnterMethod(LogLevel logLevel, string methodName);
		void EnterMethod(object instance, string methodName);
		void EnterMethod(LogLevel logLevel, object instance, string methodName);

		void LeaveMethod(LogLevel logLevel, string methodName);
		void LeaveMethod(string methodName);
		void LeaveMethod(object instance, string methodName);
		void LeaveMethod(LogLevel logLevel, object instance, string methodName);

		void LogException(Exception exception);
		void LogException(string message, Exception exception);

		void LogWarning(string message);

		void LogString(string name, string value);

		void LogString(LogLevel level, string name, string value);

		void LogInt(string name, int value);
		
		void LogInt(LogLevel level, string name, int value);

	}
}