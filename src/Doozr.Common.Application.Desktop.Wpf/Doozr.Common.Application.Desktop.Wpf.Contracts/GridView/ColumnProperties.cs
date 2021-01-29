using PropertyChanged;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	[AddINotifyPropertyChangedInterface]
	public class ColumnProperties
	{
		public ColumnProperties(string id)
		{
			Id = id;
		}

		public string Id { get; }

		public double Width { get; set; }

		public bool Hidden{ get; set; }
	}
}
