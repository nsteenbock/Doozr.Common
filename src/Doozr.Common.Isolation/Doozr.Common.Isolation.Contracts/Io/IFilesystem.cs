namespace Doozr.Common.Isolation.Io
{
	public interface IFilesystem
	{
		string[] GetFiles(string path, string searchPattern);

		string[] GetFilesRecursive(string path, string searchPattern);

		string ReadAllText(string filepath);
	}
}
