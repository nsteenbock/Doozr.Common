using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Doozr.Common.Application
{
	public class ApplicationProperties : IApplicationProperties
	{
		private readonly Process process;
		private readonly Assembly assembly;
		private readonly IPortableApplicationDetector portableApplicationDetector;

		public ApplicationProperties(IPortableApplicationDetector portableApplicationDetector)
		{
			process = Process.GetCurrentProcess();
			assembly = Assembly.GetEntryAssembly();
			this.portableApplicationDetector = portableApplicationDetector;
		}

		public string ProcessName => process.ProcessName;

		public string RootDirectory => Path.GetDirectoryName(assembly.Location);

		public string AppDataDirectory 
		{
			get{
				if (portableApplicationDetector.IsPortableApplication)
				{
					return Path.Combine(RootDirectory, "PortableAppData");
				}

				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ProcessName);
			}
		}
	}
}
