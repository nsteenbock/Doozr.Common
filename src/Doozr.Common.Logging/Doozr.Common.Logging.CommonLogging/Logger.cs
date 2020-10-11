using Common.Logging;
using System;

namespace Doozr.Common.Logging.CommonLogging
{
	public class Logger: Doozr.Common.Logging.Logger
	{
		private readonly global::Common.Logging.ILog logger;

		private const LogLevel defaultLogLevel = LogLevel.Info;

		public Logger(global::Common.Logging.ILog logger)
		{
			this.logger = logger;
		}


		public override void EnterMethod(string methodName)
		{
			EnterMethod(defaultLogLevel, methodName);
		}

		public override void EnterMethod(LogLevel logLevel, string methodName)
		{
			Log(logLevel, $"-> EnterMethod: {methodName}");
		}

		public override void EnterMethod(object instance, string methodName)
		{
			EnterMethod(defaultLogLevel, instance, methodName);
		}

		public override void EnterMethod(LogLevel logLevel, object instance, string methodName)
		{
			EnterMethod(logLevel, $"{instance.GetType()}.{methodName}");
		}

		public override void LeaveMethod(LogLevel logLevel, string methodName)
		{
			Log(logLevel, $"<- LeaveMethod: {methodName}");
		}

		public override void LeaveMethod(string methodName)
		{
			LeaveMethod(defaultLogLevel, methodName);
		}

		public override void LeaveMethod(object instance, string methodName)
		{
			LeaveMethod(defaultLogLevel, instance, methodName);
		}

		public override void LeaveMethod(LogLevel logLevel, object instance, string methodName)
		{
			LeaveMethod(logLevel, $"{instance.GetType()}.{methodName}");
		}

		public override void Log(string message)
		{
			Log(LogLevel.Info, message);
		}

		public override void Log(LogLevel logLevel, string message)
		{
			switch(logLevel)
			{
				case LogLevel.Debug:
					logger.Trace(message);
					break;
				case LogLevel.Verbose:
					logger.Debug(message);
					break;
				case LogLevel.Info:
					logger.Info(message);
					break;
				case LogLevel.Warning:
					logger.Warn(message);
					break;
				case LogLevel.Error:
					logger.Error(message);
					break;
				case LogLevel.Fatal:
					logger.Fatal(message);
					break;
				default:
					logger.Warn($"Logging error: LogLevel unknown - {logLevel}");
					break;
			}
		}

		public override void LogException(Exception exception)
		{
			Log(LogLevel.Error, GetExceptionString(exception));
		}

		private static string GetExceptionString(Exception exception, string customMessage = "")
		{
			return $"Exception {customMessage}{Environment.NewLine}  Message: {exception.Message}{Environment.NewLine}  Type: {exception.GetType()}{Environment.NewLine}  StackTrace:{exception.StackTrace}";
		}

		public override void LogException(string message, Exception exception)
		{
			Log(LogLevel.Error, GetExceptionString(exception, message));
		}

		public override void LogWarning(string message)
		{
			Log(LogLevel.Warning, message);
		}


		public override void LogString(LogLevel level, string name, string value)
		{
			Log(level, $"{name} = {value}");
		}

		public override void LogString(string name, string value)
		{
			Log($"{name} = {value}");
		}

		public override void LogInt(string name, int value)
		{
			Log($"{name} = {value}");
		}

		public override void LogInt(LogLevel level, string name, int value)
		{
			Log(level, $"{name} = {value}");
		}
	}
}
