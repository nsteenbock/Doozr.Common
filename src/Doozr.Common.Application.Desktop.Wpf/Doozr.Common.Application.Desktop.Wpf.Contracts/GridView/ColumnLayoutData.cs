using PropertyChanged;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	[AddINotifyPropertyChangedInterface]
	public class ColumnLayoutData
	{
		public ColumnProperties[] Columns{ get; set; }

		public string AutoSizeColumn{ get; set; }

		public SortColumn[] SortColumns{ get; set; }
	}
}
