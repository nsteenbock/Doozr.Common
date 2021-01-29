using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Doozr.Common.Application.Desktop.Wpf.Behaviors
{
	public class ListViewColumnManager
	{
		public ListViewColumnManager(ListView listView)
		{
			this.listView = listView;
			this.listView.Loaded += ListView_Loaded1;
			this.listView.Unloaded += ListView_Unloaded;
		}

		private void ListView_Unloaded(object sender, RoutedEventArgs e)
		{
			
		}

		private void ListView_Loaded1(object sender, RoutedEventArgs e)
		{
			if (listView.View is System.Windows.Controls.GridView gridView)
			{
				var scrollViewer = listView.GetVisualChild<ScrollViewer>();
				listView.SizeChanged += ListView_SizeChanged;
			}
		}

		private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (listView.View is System.Windows.Controls.GridView gridView)
			{
				var actualWidth = listView.ActualWidth;
				var scrollViewer = listView.GetVisualChild<ScrollViewer>();
				var viewPortWidth = scrollViewer.ViewportWidth;
				var allColumnsWidth = gridView.Columns.Sum(x => x.ActualWidth);
				var lastColumn = gridView.Columns.Last();
				var lastColumnWidth = lastColumn.ActualWidth;
				lastColumn.Width = viewPortWidth - allColumnsWidth + lastColumnWidth;
				scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
			}
		}

		public static bool GetEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(EnabledProperty);
		}

		public static void SetEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(EnabledProperty, value);
		}

		// Using a DependencyProperty as the backing store for Enabled.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty EnabledProperty =
			DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(ListViewColumnManager), new PropertyMetadata(OnSetEnabledCallback));
		private readonly ListView listView;

		private static void OnSetEnabledCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ListView listView)
			{
				var manager = new ListViewColumnManager(listView);
			}

			//gridView.Columns.CollectionChanged += Columns_CollectionChanged;
			//gridView.GetVisualParent<ListView>().SizeChanged += GridViewColumnManager_SizeChanged;
		}

		private static void GridViewColumnManager_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			
		}
		/*
		public static T GetVisualParent<T>(this DependencyObject depObj) where T : DependencyObject
		{
			if (VisualTreeHelper.GetParent(depObj) is DependencyObject parent)
			{
				var result = (parent as T) ?? GetVisualParent<T>(parent);
				if (result != null)
					return result;
			}

			return null;
		}*/
		private static void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			
		}

		private class GridViewColumnManagerBehavior
		{
			
		}
	}
}
