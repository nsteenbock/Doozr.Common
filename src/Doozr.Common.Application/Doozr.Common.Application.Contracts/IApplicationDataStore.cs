namespace Doozr.Common.Application
{
	public interface IApplicationDataStore
	{
		void WriteFile(string path, string content);

		string ReadFile(string path);

		bool FileExists(string path);
	}
}
