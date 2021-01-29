using System;

namespace Doozr.Common.Application.Desktop.Wpf.Exceptions
{
	public class InvalidAttachedPropertyUsageException: Exception
	{
		public InvalidAttachedPropertyUsageException(Type actualType, Type validType, string propertyName)
		:base($@"Attached property ""{
			(propertyName.EndsWith("Property")? 
				propertyName.Replace("Property", "")
				: propertyName)
			}"" is only valid for {validType.FullName} and not for {actualType.FullName}.")
		{

		}
	}
}
