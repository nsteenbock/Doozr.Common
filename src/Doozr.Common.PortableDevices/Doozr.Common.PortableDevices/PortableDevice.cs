using MediaDevices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Doozr.Common.PortableDevices
{
	public class PortableDevice : IPortableDevice
	{
		private readonly MediaDevice mediaDevice;

		public PortableDevice(MediaDevice mediaDevice)
		{
			this.mediaDevice = mediaDevice;
		}

		public string DeviceId => mediaDevice.DeviceId;
		public string FriendlyName => mediaDevice.FriendlyName;

		public void Sync(string portableMediaDevicePath, string targetPath, IEnumerable<string> knownPersistentUniqueIdentifiers, Action<SyncProgress> syncProgressCallback = null)
		{
			if (!mediaDevice.IsConnected) mediaDevice.Connect();

			//var a = mediaDevice.GetDirectoryInfo(@"\");

			var sourceDirectoryInfo = mediaDevice.GetDirectoryInfo(portableMediaDevicePath);

			var files = sourceDirectoryInfo.EnumerateFiles().ToArray();


			var totalFiles = files.Count();
			var totalBytes = files.Sum(x => (Int64)x.Length);
			var filesToSkip = files.Where(x => knownPersistentUniqueIdentifiers.Contains(x.PersistentUniqueId));

			var syncedFiles = filesToSkip.Count();
			var syncedBytes = filesToSkip.Sum(x => (Int64)x.Length);

			syncProgressCallback?.Invoke(new SyncProgress
			{
				TotalFiles = totalFiles,
				TotalBytes = totalBytes,
				SyncedFiles = syncedFiles,
				SyncedBytes = syncedBytes,
				LastSyncedFiles = filesToSkip.Select(x => new SyncFile { PersistentUniqueId = x.PersistentUniqueId, Status = SyncFile.SyncFileStatus.Skipped })
			});

			var filesToSync = files.Where(x => !knownPersistentUniqueIdentifiers.Contains(x.PersistentUniqueId));
			foreach(var fileToSync in filesToSync)
			{
				// SyncFile
				mediaDevice.DownloadFileFromPersistentUniqueId(fileToSync.PersistentUniqueId, Path.Combine(targetPath, fileToSync.Name));

				syncedFiles++;
				syncedBytes += (Int64)fileToSync.Length;
				syncProgressCallback?.Invoke(new SyncProgress
				{
					TotalFiles = totalFiles,
					TotalBytes = totalBytes,
					SyncedFiles = syncedFiles,
					SyncedBytes = syncedBytes,
					LastSyncedFiles = new SyncFile[] { new SyncFile { PersistentUniqueId = fileToSync.PersistentUniqueId, Status = SyncFile.SyncFileStatus.Success } }
				});
			}
		}
	}
}
