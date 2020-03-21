using System;

namespace Doozr.Common.Wmi
{
	public interface IWmiObject
	{
		DateTime GetUtcDateTime(string propertyName);

		string GetString(string propertyName);
	}
}
