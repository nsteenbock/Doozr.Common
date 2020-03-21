using System.Collections.Generic;

namespace Doozr.Common.Wmi
{
	public interface IWmiManager
	{
		IWmiObject QuerySingleton(string queryString);

		IEnumerable<IWmiObject> Query(string queryString);
	}
}
