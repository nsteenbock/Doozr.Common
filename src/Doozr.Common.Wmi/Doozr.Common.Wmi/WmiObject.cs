using System;
using System.Management;

namespace Doozr.Common.Wmi
{
	public class WmiObject: IWmiObject
	{
		private readonly ManagementObject managementObject;

		public WmiObject(ManagementObject managementObject)
		{
			this.managementObject = managementObject;
		}

		public string GetString(string propertyName)
		{
			return managementObject[propertyName].ToString();
		}

		public DateTime GetUtcDateTime(string propertyName)
		{
			return ManagementDateTimeConverter.ToDateTime(managementObject[propertyName].ToString())
				.ToUniversalTime();
		}
	}
}
