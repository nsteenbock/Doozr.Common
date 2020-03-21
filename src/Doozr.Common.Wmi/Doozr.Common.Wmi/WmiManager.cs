using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Doozr.Common.Wmi
{
	public class WmiManager : IWmiManager
	{
		public IEnumerable<IWmiObject> Query(string queryString)
		{
			return QueryWmi(queryString).Select(x => new WmiObject(x));
		}

		public IWmiObject QuerySingleton(string queryString)
		{
			return QueryWmi(queryString).Select(x => new WmiObject(x)).Single();
		}

		IEnumerable<ManagementObject> QueryWmi(string queryString)
		{
			return new ManagementObjectSearcher(queryString).Get().OfType<ManagementObject>();
		}
	}
}