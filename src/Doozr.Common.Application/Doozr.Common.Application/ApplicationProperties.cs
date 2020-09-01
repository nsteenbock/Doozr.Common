using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Doozr.Common.Application
{
	public class ApplicationProperties : IApplicationProperties
	{
		private readonly Process process;
		private readonly Assembly assembly;

		public ApplicationProperties()
		{
			process = Process.GetCurrentProcess();
			assembly = Assembly.GetEntryAssembly();
		}

		public string ProcessName => process.ProcessName;

		public string RootDirectory => Path.GetDirectoryName(assembly.Location);
	}
}
