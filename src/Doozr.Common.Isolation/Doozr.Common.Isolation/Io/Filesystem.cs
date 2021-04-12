using System.IO;

namespace Doozr.Common.Isolation.Io
{
	public class FileSystem : IFileSystem
	{
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		public string[] GetDirectories(string path)
		{
			return Directory.GetDirectories(path);
		}

		public FileInfo GetFileInfo(string path)
		{
			var systemIoFileInfo = new System.IO.FileInfo(path);

			return systemIoFileInfo.ToIsolationFileInfo();
		}

		public string[] GetFiles(string path, string searchPattern)
		{
			return Directory.GetFiles(path, searchPattern);
		}

		public string[] GetFiles(string path)
		{
			return Directory.GetFiles(path);
		}

		public string[] GetFilesRecursive(string path, string searchPattern)
		{
			return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
		}

		public string ReadAllText(string filepath)
		{
			return File.ReadAllText(filepath);
		}
	}
}
