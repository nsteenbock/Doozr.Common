using Doozr.Common.Logging;
using Doozr.Common.Logging.Aspect;
using System;
using System.IO;

namespace Doozr.Common.Application
{
	[Log]
	public class ApplicationDataStore: IApplicationDataStore, ILoggingObject
	{
		private readonly IApplicationProperties applicationProperties;

		public ILogger Logger { get; set; }

		public ApplicationDataStore(IApplicationProperties applicationProperties)
		{
			this.applicationProperties = applicationProperties;
		}

		public string ReadFile(string path)
		{
			return File.ReadAllText(GetCompletePath(path));
		}

		public void WriteFile(string path, string content)
		{
			Logger?.LogString("content", content);

			var completePath = GetCompletePath(path);
			var directory = Path.GetDirectoryName(completePath);
			if (!Directory.Exists(directory))
			{
				Logger?.Log($"Creating directory: {directory}");
				Directory.CreateDirectory(directory);
			}
			File.WriteAllText(completePath, content);
		}

		private string GetCompletePath(string path)
		{
			Logger?.LogString("path", path);
			var result = Path.Combine(applicationProperties.AppDataDirectory, path);
			Logger?.LogString("result", result);
			return result;
		}

		private string GetApplicationDataPath(bool isPortableApplication, IApplicationProperties applicationProperties)
		{
			if (isPortableApplication)
			{
				return applicationProperties.RootDirectory;
			}
			else
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), applicationProperties.ProcessName);
			}
		}
	}
}