using Doozr.Common.Isolation.Io;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Doozr.Common.Utilities.Filesystem
{
	public interface IFileSearcher
	{
		string[] FindFiles(IEnumerable<string> searchPaths, Func<FileInfo, bool> matchFunction, Action<FileSearchProgress> progressCallback = null, CancellationToken cancellationToken = default);
	}
}
