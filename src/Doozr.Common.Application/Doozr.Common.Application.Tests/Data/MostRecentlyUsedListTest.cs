using Doozr.Common.Application.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Doozr.Common.Application.Tests.Data
{
	[TestClass]
	public class MostRecentlyUsedListTest
	{
		[TestMethod]
		public void Count_JustCreatedList()
		{
			var sut = new MostRecentlyUsedList<int>();

			Assert.AreEqual(0, sut.Items.Count());
		}

		[TestMethod]
		public void Add_AddOneItem()
		{
			var sut = new MostRecentlyUsedList<int>();
			sut.Add(1);

			Assert.AreEqual(1, sut.Items.Count());
			Assert.AreEqual("1", Serialize(sut.Items));
		}

		[TestMethod]
		public void Add_AddThreeDifferentItems()
		{
			var sut = new MostRecentlyUsedList<int>();
			sut.Add(1);
			sut.Add(2);
			sut.Add(3);

			Assert.AreEqual(3, sut.Items.Count());
			Assert.AreEqual("3,2,1", Serialize(sut.Items));
		}

		[TestMethod]
		public void Add_AddThreeItemsOnlyTwoDiffer()
		{
			var sut = new MostRecentlyUsedList<int>();
			sut.Add(1);
			sut.Add(2);
			sut.Add(1);

			Assert.AreEqual(2, sut.Items.Count());
			Assert.AreEqual("1,2", Serialize(sut.Items));
		}

		[TestMethod]
		public void Add_ItemsExceedCapacity()
		{
			var sut = new MostRecentlyUsedList<int>();
			sut.Capacity = 2;
			sut.Add(1);
			sut.Add(2);
			sut.Add(3);

			Assert.AreEqual(2, sut.Items.Count());
			Assert.AreEqual("3,2", Serialize(sut.Items));
		}

		[TestMethod]
		public void Capacity_ChangedToValueLowerThanCurrentCountOfItems()
		{
			var sut = new MostRecentlyUsedList<int>();
			sut.Capacity = 10;
			sut.Add(1);
			sut.Add(2);
			sut.Add(3);
			sut.Add(4);
			sut.Add(5);

			sut.Capacity = 3;

			Assert.AreEqual("5,4,3", Serialize(sut.Items));
		}

		[TestMethod]
		public void PropertyChanged_AddOneItem()
		{
			var propertyChangedCalls = 0;
			var sut = new MostRecentlyUsedList<int>();
			sut.PropertyChanged += (sender, args) => propertyChangedCalls++;

			sut.Add(1);

			Assert.AreEqual(1, propertyChangedCalls);
		}

		[TestMethod]
		public void PropertyChanged_AddTwoItems()
		{
			var propertyChangedCalls = 0;
			var sut = new MostRecentlyUsedList<int>();
			sut.PropertyChanged += (sender, args) => propertyChangedCalls++;

			sut.Add(1);
			sut.Add(2);

			Assert.AreEqual(2, propertyChangedCalls);
		}

		[TestMethod]
		public void PropertyChanged_AddThreeItemsWhereSecondAndThirdAreTheSame()
		{
			var propertyChangedCalls = 0;
			var sut = new MostRecentlyUsedList<int>();
			sut.PropertyChanged += (sender, args) => propertyChangedCalls++;

			sut.Add(1);
			sut.Add(2);
			sut.Add(2);

			Assert.AreEqual(3, propertyChangedCalls);
		}

		[TestMethod]
		public void PropertyChanged_ChangeCapacityToValueGreaterThanCurrentItemCount()
		{
			var propertyChangedCallsForItems = 0;
			var propertyChangedCallsForCapacity = 0;
			var sut = new MostRecentlyUsedList<int>();
			sut.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(sut.Items)) propertyChangedCallsForItems++;
				if (args.PropertyName == nameof(sut.Capacity)) propertyChangedCallsForCapacity++;
			};

			sut.Add(1);
			sut.Add(2);
			sut.Capacity = 5;

			Assert.AreEqual(2, propertyChangedCallsForItems);
			Assert.AreEqual(1, propertyChangedCallsForCapacity);
		}

		[TestMethod]
		public void PropertyChanged_ChangeCapacityToValueLowerThanCurrentItemCount()
		{
			var propertyChangedCallsForItems = 0;
			var propertyChangedCallsForCapacity = 0;
			var sut = new MostRecentlyUsedList<int>();
			sut.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(sut.Items)) propertyChangedCallsForItems++;
				if (args.PropertyName == nameof(sut.Capacity)) propertyChangedCallsForCapacity++;
			};

			sut.Add(1);
			sut.Add(2);
			sut.Add(3);
			sut.Capacity = 2;

			Assert.AreEqual(4, propertyChangedCallsForItems);
			Assert.AreEqual(1, propertyChangedCallsForCapacity);
		}

		[TestMethod]
		public void PropertyChanged_ChangeItems()
		{
			var propertyChangedCallsForItems = 0;
			var sut = new MostRecentlyUsedList<int>();
			sut.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(sut.Items)) propertyChangedCallsForItems++;
			};

			sut.Items = new[] { 1, 2 };

			Assert.AreEqual(1, propertyChangedCallsForItems);
		}

		[TestMethod]
		public void PropertyChanged_ChangeItemsToNull()
		{
			var propertyChangedCallsForItems = 0;
			var sut = new MostRecentlyUsedList<int>();
			sut.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(sut.Items)) propertyChangedCallsForItems++;
			};

			sut.Items = null;

			Assert.AreEqual(1, propertyChangedCallsForItems);
		}


		[TestMethod]
		public void Serialize()
		{
			var sut = new MostRecentlyUsedList<int>();
			sut.Add(1);
			sut.Add(2);

			var s = new JsonObjectSerializer().Serialize(sut);
		}

		

		private string Serialize<T>(T[] values)
		{
			return string.Join(',', values);
		}
	}
}
