using Newtonsoft.Json;
using System;

namespace Doozr.Common.Application
{
	public class JsonObjectSerializer : IObjectSerializer
	{
		public T Deserialize<T>(string serializedObject)
		{
			return JsonConvert.DeserializeObject<T>(serializedObject);
		}

		public object Deserialize(string serializedObject, Type targetType)
		{
			return JsonConvert.DeserializeObject(serializedObject, targetType);
		}

		public string Serialize(object value)
		{
			return JsonConvert.SerializeObject(value, Formatting.Indented);
		}
	}
}
