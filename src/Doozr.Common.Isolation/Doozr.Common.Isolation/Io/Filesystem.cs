using System.IO;

namespace Doozr.Common.Isolation.Io
{
	public class Filesystem : IFilesystem
	{
		public string[] GetFiles(string path, string searchPattern)
		{
			return Directory.GetFiles(path, searchPattern);
		}

		public string[] GetFilesRecursive(string path, string searchPattern)
		{
			return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
		}
	}
}
