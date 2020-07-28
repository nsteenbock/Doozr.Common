using System;
using System.Collections;
using System.Collections.Generic;

namespace Doozr.Common.PortableDevices
{
	public interface IPortableDevice
	{
		string DeviceId{ get; }

		string FriendlyName{ get; }

		void Sync(string portableDevicePath, string targetPath, IEnumerable<string> knownPersistentUniqueIdentifiers, Action<SyncProgress> syncProgressCallback = null);
	}
}
