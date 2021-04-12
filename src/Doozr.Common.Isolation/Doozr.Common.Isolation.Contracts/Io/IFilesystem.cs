namespace Doozr.Common.Isolation.Io
{
	public interface IFileSystem
	{
		string[] GetFiles(string path, string searchPattern);

		string[] GetFiles(string path);

		string[] GetDirectories(string path);

		string[] GetFilesRecursive(string path, string searchPattern);

		string ReadAllText(string filepath);

		bool FileExists(string path);

		bool DirectoryExists(string path);

		FileInfo GetFileInfo(string path);
	}
}
