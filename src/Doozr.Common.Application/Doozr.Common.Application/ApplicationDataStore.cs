using Doozr.Common.Logging.Aspect;
using System;
using System.IO;

namespace Doozr.Common.Application
{
	[Logging]
	public class ApplicationDataStore: IApplicationDataStore
	{
		private readonly string applicationDataPath;

		public ApplicationDataStore(IPortableApplicationDetector portableApplicationDetector, IApplicationProperties applicationProperties)
		{
			applicationDataPath = GetApplicationDataPath(portableApplicationDetector.IsPortableApplication, applicationProperties);
		}

		public string ReadFile(string path)
		{
			return File.ReadAllText(GetCompletePath(path));
		}

		public void WriteFile(string path, string content)
		{
			var completePath = GetCompletePath(path);
			var directory = Path.GetDirectoryName(completePath);
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			File.WriteAllText(completePath, content);
		}

		private string GetCompletePath(string path)
		{
			return Path.Combine(applicationDataPath, path);
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
