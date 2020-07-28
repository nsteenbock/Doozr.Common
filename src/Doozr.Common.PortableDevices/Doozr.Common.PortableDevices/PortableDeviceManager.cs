using MediaDevices;
using System.Collections.Generic;
using System.Linq;

namespace Doozr.Common.PortableDevices
{
	public class PortableDeviceManager : IPortableDeviceManager
	{
		IEnumerable<IPortableDevice> IPortableDeviceManager.GetDevices()
		{
			return MediaDevice.GetDevices().Select(x => new PortableDevice(x));
		}
	}
}
