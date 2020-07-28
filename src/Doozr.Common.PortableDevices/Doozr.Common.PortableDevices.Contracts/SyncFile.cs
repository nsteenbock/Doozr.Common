using System;
using System.Collections.Generic;
using System.Text;

namespace Doozr.Common.PortableDevices
{
	public class SyncFile
	{
		public string PersistentUniqueId{ get; set; }

		public SyncFileStatus Status{ get; set; }

		public enum SyncFileStatus { Skipped, Success }
	}
}
