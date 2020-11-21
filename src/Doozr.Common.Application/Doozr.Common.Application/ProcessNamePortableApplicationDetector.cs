using System.Diagnostics;

namespace Doozr.Common.Application
{
	public class ProcessNamePortableApplicationDetector : IPortableApplicationDetector
	{
		public bool IsPortableApplication => Process.GetCurrentProcess().ProcessName.EndsWith("Portable");
	}
}
