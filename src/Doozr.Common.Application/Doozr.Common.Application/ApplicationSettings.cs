using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Doozr.Common.Application
{
	public class ApplicationSettings : IApplicationSettings
	{
		private readonly string settingsPath;
		private readonly IApplicationDataStore applicationDataStore;
		private readonly IObjectSerializer objectSerializer;

		private readonly Dictionary<Type, Settings> settings = new Dictionary<Type, Settings>();

		public ApplicationSettings(
			string settingsPath,
			IApplicationDataStore applicationDataStore,
			IObjectSerializer objectSerializer)
		{
			this.settingsPath = settingsPath;
			this.applicationDataStore = applicationDataStore;
			this.objectSerializer = objectSerializer;
		}

		public T Get<T>()
		{
			var settingType = typeof(T);
			if (!settings.ContainsKey(settingType)) throw new InvalidOperationException($"Setting with type '{settingType}' not registered.");

			var setting = settings[settingType];

			if (setting.SettingObject == null)
			{
				if (!applicationDataStore.FileExists(Path.Combine(settingsPath, setting.Path)))
				{
					setting.SettingObject = Activator.CreateInstance(settingType, null);
					setting.IsDirty = true;
				}
				else
				{
					var serializedSetting = applicationDataStore.ReadFile(Path.Combine(settingsPath, setting.Path));
					setting.SettingObject = objectSerializer.Deserialize(
						serializedSetting, typeof(T));
				}
			}

			return (T)settings[settingType].SettingObject;
		}

		public void Register<T>(string path)
		{
			var settingType = typeof(T);
			if (settings.ContainsKey(settingType)) throw new InvalidOperationException($"Setting of type '{settingType}' already registered.");

			var settingWithSamePath = settings.Where(x => x.Value.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase));
			if (settingWithSamePath.Any()) throw new InvalidOperationException($"Setting of type '{settingWithSamePath.First().Key}' already uses path '{settingWithSamePath.First().Value.Path}'.");

			settings.Add(typeof(T), new Settings { Path = path });
		}

		class Settings
		{
			public string Path { get; set; }

			public object SettingObject{ get; set; }

			public bool IsDirty{ get; set; }
		}

		public void Save()
		{
			foreach(var setting in settings)
			{
				if (setting.Value.SettingObject != null && 
					(setting.Value.IsDirty || !(typeof(INotifyPropertyChanged).IsAssignableFrom(setting.Key))))
				{
					applicationDataStore.WriteFile(Path.Combine(settingsPath, setting.Value.Path), objectSerializer.Serialize(setting.Value.SettingObject));
				}
			}
		}
	}
}
