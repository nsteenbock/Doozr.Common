using Doozr.Common.Isolation.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Doozr.Common.Utilities.Filesystem
{
	public class FileSearcher : IFileSearcher
	{
		private readonly IFileSystem filesystem;

		public FileSearcher(IFileSystem filesystem)
		{
			this.filesystem = filesystem;
		}

		public string[] FindFiles(IEnumerable<string> searchPaths, Func<FileInfo, bool> matchFunction, Action<FileSearchProgress> progressCallback = null, CancellationToken cancellationToken = default)
		{
			var result = new List<string>();
			var progress = new FileSearchProgress();

			InternalFindFiles(result, searchPaths, matchFunction, progressCallback, progress, cancellationToken);

			return result.ToArray();
		}

		private void InternalFindFiles(List<string> result, IEnumerable<string> searchPaths, Func<FileInfo, bool> matchFunction, Action<FileSearchProgress> progressCallback, FileSearchProgress progress, CancellationToken cancellationToken)
		{
			foreach (var searchPath in searchPaths)
			{
				cancellationToken.ThrowIfCancellationRequested();

				if (filesystem.FileExists(searchPath))
				{
					HandleFile(result, matchFunction, progress, searchPath);
				}
				else
				{
					if (filesystem.DirectoryExists(searchPath))
					{
						HandleDirectory(result, matchFunction, progressCallback, progress, searchPath, cancellationToken);
					}
					else
					{
						progress.UnresolvablePaths++;
					}
				}

				progressCallback?.Invoke(progress);
			}
		}

		private void HandleDirectory(List<string> result, Func<FileInfo, bool> matchFunction, Action<FileSearchProgress> progressCallback, FileSearchProgress progress, string searchPath, CancellationToken cancellationToken)
		{
			try
			{
				InternalFindFiles(result,
					filesystem.GetFiles(searchPath).Union(filesystem.GetDirectories(searchPath)),
					matchFunction,
					progressCallback,
					progress,
					cancellationToken
					);
			}
			catch (UnauthorizedAccessException)
			{
				progress.UnauthorizedAccesses++;
			}
		}

		private void HandleFile(List<string> result, Func<FileInfo, bool> matchFunction, FileSearchProgress progress, string searchPath)
		{
			if (matchFunction(filesystem.GetFileInfo(searchPath)))
			{
				result.Add(searchPath);
				progress.FoundFiles++;
			}
		}
	}
}
