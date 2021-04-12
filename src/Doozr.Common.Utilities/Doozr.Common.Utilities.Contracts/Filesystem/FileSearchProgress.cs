namespace Doozr.Common.Utilities.Filesystem
{
	public class FileSearchProgress
	{
		public int UnresolvablePaths{ get; set; }

		public int FoundFiles { get; set; }

		public int UnauthorizedAccesses { get; set; }
	}
}
