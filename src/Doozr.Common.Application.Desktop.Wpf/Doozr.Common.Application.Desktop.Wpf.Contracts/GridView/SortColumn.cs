using PropertyChanged;
using System.ComponentModel;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	[AddINotifyPropertyChangedInterface]
	public class SortColumn
	{
		public SortColumn()
		{

		}
		public string Id { get; set; }

		public ListSortDirection SortDirection { get; set; }
	}
}
