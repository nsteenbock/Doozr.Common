using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	[AddINotifyPropertyChangedInterface]
	public class SortColumn
	{
		public string Id{ get; set; }

		public ListSortDirection SortDirection{ get; set; }
	}

	[TypeConverter(typeof(SortColumnListConverter))]
	public class SortColumnList : List<SortColumn>
	{
		public SortColumnList()
		{
			
		}
	}
}
