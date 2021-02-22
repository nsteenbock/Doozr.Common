using System.Collections.Generic;
using System.ComponentModel;

namespace Doozr.Common.Application.Data
{
	public class MostRecentlyUsedList<T> : INotifyPropertyChanged
	{
		private List<T> items = new List<T>();

		private int capacity = 10;

		public event PropertyChangedEventHandler PropertyChanged;

		public int Capacity
		{
			get => capacity;
			set
			{
				var itemsChanged = false;
				capacity = value;
				while (capacity < items.Count)
				{
					items.RemoveAt(items.Count - 1);
					itemsChanged = true;
				}

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Capacity)));
				if (itemsChanged) PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
			}
		}
		public void Add(T item)
		{
			if (items.Contains(item))
			{
				items.Remove(item);
			}
			items.Insert(0, item);

			if (items.Count > Capacity)
			{
				items.RemoveAt(items.Count - 1);
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
		}

		public T[] Items
		{
			get => items.ToArray();
			set
			{
				items.Clear();
				if (value != null)
				{
					items.AddRange(value);
				}

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
			}
		}
	}
}
