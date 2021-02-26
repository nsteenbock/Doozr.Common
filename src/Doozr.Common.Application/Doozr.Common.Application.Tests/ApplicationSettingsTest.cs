using Doozr.Common.Application.Tests.Helper;
using Doozr.Common.Application.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Doozr.Common.Application.Tests
{
	[TestClass]
	public class ApplicationSettingsTest
	{
		IApplicationDataStore applicationDataStore;
		IObjectSerializer objectSerializer;
		ApplicationSettings sut;

		const string SETTING_PATH = "settingPath";

		[TestMethod]
		public void Register_Type_Once()
		{
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");

			// Just expect that operation succeeds without exception.
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Register_Type_Twice()
		{
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			sut.Register<SettingWithNotifyPropertyChanged>("setting2");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Register_Different_Types_With_Same_SettingPath()
		{
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			sut.Register<AnotherSettingWithNotifyPropertyChanged>("setting1");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Get_UnregisteredSettingType()
		{
			sut.Get<SettingWithNotifyPropertyChanged>();
		}

		[TestMethod]
		public void Get_RegisteredSettingType_Not_Persisted()
		{
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			var resolvedSettingObject = sut.Get<SettingWithNotifyPropertyChanged>();

			Assert.IsNotNull(resolvedSettingObject);
		}

		[TestMethod]
		public void Get_RegisteredSettingType_PersistedDataExists()
		{
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			applicationDataStore.WriteFile(Path.Combine(SETTING_PATH, "setting1"), @"{ ""Value"": 23 }");
			var resolvedSettingObject = sut.Get<SettingWithNotifyPropertyChanged>();

			Assert.IsNotNull(resolvedSettingObject);
			Assert.AreEqual(23, resolvedSettingObject.Value);
		}

		[TestMethod]
		public void Save_Registered_Setting_Not_Resolved_At_Any_Time()
		{
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			sut.Save();
			Assert.IsFalse(applicationDataStore.FileExists(Path.Combine(SETTING_PATH, "setting1")));
		}

		[TestMethod]
		public void Save_After_Resolve_Registered_Setting()
		{
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			var settingValue = sut.Get<SettingWithNotifyPropertyChanged>();
			sut.Save();
			Assert.IsTrue(applicationDataStore.FileExists(Path.Combine(SETTING_PATH, "setting1")));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Save_Registered_Setting_Without_NotifyPropertyChange()
		{
			sut.Register<SettingWithoutNotifyPropertyChanged>("setting1");
		}

		[TestMethod]
		public void Save_RegisteredSettingWithNotifyPropertyChange_ChangingPersistedDataWithoutTouchingSettingObject()
		{
			applicationDataStore.WriteFile(Path.Combine(SETTING_PATH, "setting1"), @"{ ""Value"": 12 }");
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			var resolvedValue = sut.Get<SettingWithNotifyPropertyChanged>();
			applicationDataStore.WriteFile(Path.Combine(SETTING_PATH, "setting1"), @"{ ""Value"": 13 }");

			sut.Save();

			Assert.AreEqual(13, objectSerializer.Deserialize<SettingWithNotifyPropertyChanged>(applicationDataStore.ReadFile(Path.Combine(SETTING_PATH, "setting1"))).Value);
		}

		[TestMethod]
		public void Save_RegisteredSettingWithNotifyPropertyChange_ChangingSettingObject()
		{
			applicationDataStore.WriteFile(Path.Combine(SETTING_PATH, "setting1"), @"{ ""Value"": 12 }");
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			var resolvedValue = sut.Get<SettingWithNotifyPropertyChanged>();
			applicationDataStore.WriteFile(Path.Combine(SETTING_PATH, "setting1"), @"{ ""Value"": 13 }");

			resolvedValue.Value = 14;

			Assert.AreEqual(14, objectSerializer.Deserialize<SettingWithNotifyPropertyChanged>(applicationDataStore.ReadFile(Path.Combine(SETTING_PATH, "setting1"))).Value);
		}

		[TestMethod]
		public void Save_RegisteredSettingWithNotifyPropertyChange_ChangingSettingObject_CallingSaveAgainAfterAutomaticSaving()
		{
			applicationDataStore.WriteFile(Path.Combine(SETTING_PATH, "setting1"), @"{ ""Value"": 12 }");
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			var resolvedValue = sut.Get<SettingWithNotifyPropertyChanged>();
			applicationDataStore.WriteFile(Path.Combine(SETTING_PATH, "setting1"), @"{ ""Value"": 13 }");
			resolvedValue.Value = 14;
			applicationDataStore.WriteFile(Path.Combine(SETTING_PATH, "setting1"), @"{ ""Value"": 15 }");

			sut.Save();

			Assert.AreEqual(15, objectSerializer.Deserialize<SettingWithNotifyPropertyChanged>(applicationDataStore.ReadFile(Path.Combine(SETTING_PATH, "setting1"))).Value);
		}

		[TestMethod]
		public void Get_RegisteredSettingType_Twice()
		{
			sut.Register<SettingWithNotifyPropertyChanged>("setting1");
			var resolvedSettingObject1 = sut.Get<SettingWithNotifyPropertyChanged>();
			var resolvedSettingObject2 = sut.Get<SettingWithNotifyPropertyChanged>();

			Assert.AreEqual(resolvedSettingObject1, resolvedSettingObject2);
		}
		
		[TestInitialize]
		public void Initialize()
		{
			applicationDataStore = new ApplicationDataStoreMock();
			objectSerializer = new JsonObjectSerializer();
			sut = new ApplicationSettings(SETTING_PATH, applicationDataStore, objectSerializer);
		}
	}
}
