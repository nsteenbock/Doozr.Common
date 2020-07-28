using System.Collections.Generic;

namespace Doozr.Common.PortableDevices
{
	public interface IPortableDeviceManager
	{
		IEnumerable<IPortableDevice> GetDevices();
	}
}
