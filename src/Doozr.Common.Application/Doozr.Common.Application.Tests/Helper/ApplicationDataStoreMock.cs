using System.Collections.Generic;

namespace Doozr.Common.Application.Tests.Helper
{
	public class ApplicationDataStoreMock : IApplicationDataStore
	{
		public Dictionary<string, string> files = new Dictionary<string, string>();

		public bool FileExists(string path)
		{
			return files.ContainsKey(path.ToLower());
		}

		public string ReadFile(string path)
		{
			return files[path.ToLower()];
		}

		public void WriteFile(string path, string content)
		{
			files[path.ToLower()] = content;
		}
	}
}
