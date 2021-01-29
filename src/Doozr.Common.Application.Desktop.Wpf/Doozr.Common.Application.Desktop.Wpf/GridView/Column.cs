using System.Windows;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	public class Column
	{
		public static string GetId(DependencyObject obj)
		{
			return (string)obj.GetValue(IdProperty);
		}

		public static void SetId(DependencyObject obj, string value)
		{
			obj.SetValue(IdProperty, value);
		}

		public static readonly DependencyProperty IdProperty =
			DependencyProperty.RegisterAttached("Id", typeof(string), typeof(Column), new PropertyMetadata(null));




		public static bool GetHiddenByDefault(DependencyObject obj)
		{
			return (bool)obj.GetValue(HiddenByDefaultProperty);
		}

		public static void SetHiddenByDefault(DependencyObject obj, bool value)
		{
			obj.SetValue(HiddenByDefaultProperty, value);
		}

		public static readonly DependencyProperty HiddenByDefaultProperty =
			DependencyProperty.RegisterAttached("HiddenByDefault", typeof(bool), typeof(Column), new PropertyMetadata(false));


		public static string GetPropertyName(DependencyObject obj)
		{
			return (string)obj.GetValue(PropertyNameProperty);
		}

		public static void SetPropertyName(DependencyObject obj, string value)
		{
			obj.SetValue(PropertyNameProperty, value);
		}

		public static readonly DependencyProperty PropertyNameProperty =
			DependencyProperty.RegisterAttached(
				"PropertyName",
				typeof(string),
				typeof(Column),
				new UIPropertyMetadata(null)
			);
	}
}
