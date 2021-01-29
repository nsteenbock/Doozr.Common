using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	[TypeConverter(typeof(SortColumnListConverter))]
	public class SortColumnList : ObservableCollection<SortColumn>
	{
		public SortColumnList() : base() { }

		public SortColumnList(IEnumerable<SortColumn> values): base(values)
		{
			foreach(var value in values)
			{
				if (value is INotifyPropertyChanged propChanged)
				{
					propChanged.PropertyChanged += (s, e) => { this.OnPropertyChanged(new PropertyChangedEventArgs(null)); };
				}
			}
		}
	}
}
