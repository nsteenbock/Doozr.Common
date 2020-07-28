using System;
using System.Collections.Generic;

namespace Doozr.Common.PortableDevices
{
	public class SyncProgress
	{
		public int TotalFiles{ get; set; }

		public Int64 TotalBytes{ get; set; }

		public int SyncedFiles { get; set; }

		public Int64 SyncedBytes { get; set; }

		public IEnumerable<SyncFile> LastSyncedFiles{ get; set; }
	}
}