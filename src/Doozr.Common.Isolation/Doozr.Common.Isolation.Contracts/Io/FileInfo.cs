namespace Doozr.Common.Isolation.Io
{
	public class FileInfo : FileSystemInfo
	{
		public string Name{ get; set; }

		public string FullName{ get; set; }
		
		public long Length { get; set; }
	}
}
