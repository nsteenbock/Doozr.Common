using Doozr.Common.Application.Desktop.Wpf.GridView;
using System;
using System.ComponentModel;
using System.Windows;

namespace GridViewSample
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new
			{
				Data = new[]
				{
					new { Name = "Simpson", GivenName = "Homer", BirthDate = new DateTime(1956, 5, 12) },
					new { Name = "Doe", GivenName = "John", BirthDate = new DateTime(1955, 3, 31) },
					new { Name = "Doe", GivenName = "Jane", BirthDate = new DateTime(1953, 1, 06) },
					new { Name = "Smith", GivenName = "Peter", BirthDate = new DateTime(1947, 10, 5) },
					new { Name = "Miller", GivenName = "Steve", BirthDate = new DateTime(1948, 12, 3) },
					new { Name = "Parker", GivenName = "Edward", BirthDate = new DateTime(1945, 12, 24) }
				},

				ColumnLayoutData = new ColumnLayoutData
				{
					Columns = new ColumnProperties[]
					{
						new ColumnProperties("name"){Width = 150 },
						new ColumnProperties("ABC") {Width = 300 },
						new ColumnProperties("birthdate") {Width = 177}
					},
					//AutoSizeColumn = "a"
				}
			};
			a.SizeChanged += A_SizeChanged;
		}

		private void A_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//throw new NotImplementedException();
		}
	}
}
