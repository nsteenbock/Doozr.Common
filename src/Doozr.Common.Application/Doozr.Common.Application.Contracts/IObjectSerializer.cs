using System;

namespace Doozr.Common.Application
{
	public interface IObjectSerializer
	{
		string Serialize(object value);

		T Deserialize<T>(string serializedObject);

		object Deserialize(string serializedObject, Type targetType);
	}
}
