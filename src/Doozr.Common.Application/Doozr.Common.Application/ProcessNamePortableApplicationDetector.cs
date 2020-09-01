namespace Doozr.Common.Application
{
	public class ProcessNamePortableApplicationDetector : IPortableApplicationDetector
	{
		private readonly IApplicationProperties applicationProperties;

		public ProcessNamePortableApplicationDetector(IApplicationProperties applicationProperties)
		{
			this.applicationProperties = applicationProperties;
		}

		public bool IsPortableApplication => applicationProperties.ProcessName.EndsWith("Portable");
	}
}
