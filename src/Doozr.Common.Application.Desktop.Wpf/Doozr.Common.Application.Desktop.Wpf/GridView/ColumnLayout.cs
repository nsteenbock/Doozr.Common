using Doozr.Common.Application.Desktop.Wpf.Exceptions;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	public class ColumnLayout
	{
		public static ColumnLayoutData GetData(DependencyObject obj)
		{
			return (ColumnLayoutData)obj.GetValue(DataProperty);
		}

		public static void SetData(DependencyObject obj, ColumnLayoutData value)
		{
			obj.SetValue(DataProperty, value);
		}

		public static readonly DependencyProperty DataProperty =
			DependencyProperty.RegisterAttached("Data", typeof(ColumnLayoutData), typeof(ColumnLayout), new PropertyMetadata(new ColumnLayoutData(),
				DataPropertyChanged));

		private static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is ListView listView))
			{
				throw new InvalidAttachedPropertyUsageException(d.GetType(), typeof(ListView), nameof(DataProperty));
			}

			SetColumnLayoutManager(listView, new ColumnLayoutManager(listView));
		}



		internal static ColumnLayoutManager GetColumnLayoutManager(DependencyObject obj)
		{
			return (ColumnLayoutManager)obj.GetValue(ColumnLayoutManagerProperty);
		}

		internal static void SetColumnLayoutManager(DependencyObject obj, ColumnLayoutManager value)
		{
			obj.SetValue(ColumnLayoutManagerProperty, value);
		}

		internal static readonly DependencyProperty ColumnLayoutManagerProperty =
			DependencyProperty.RegisterAttached("ColumnLayoutManager", typeof(ColumnLayoutManager), typeof(ColumnLayout), new PropertyMetadata(null));






		internal static IList<GridViewColumn> GetAvailableColumns(DependencyObject obj)
		{
			return (IList<GridViewColumn>)obj.GetValue(AvailableColumnsProperty);
		}

		internal static void SetAvailableColumns(DependencyObject obj, IList<GridViewColumn> value)
		{
			obj.SetValue(AvailableColumnsProperty, value);
		}

		internal static readonly DependencyProperty AvailableColumnsProperty =
			DependencyProperty.RegisterAttached("AvailableColumns", typeof(IList<GridViewColumn>), typeof(ColumnLayout), new PropertyMetadata(new List<GridViewColumn>()));




		public static string GetDefaultAutosizeColumnId(DependencyObject obj)
		{
			return (string)obj.GetValue(DefaultAutosizeColumnIdProperty);
		}

		public static void SetDefaultAutosizeColumnId(DependencyObject obj, string value)
		{
			obj.SetValue(DefaultAutosizeColumnIdProperty, value);
		}

		public static readonly DependencyProperty DefaultAutosizeColumnIdProperty =
			DependencyProperty.RegisterAttached("DefaultAutosizeColumnId", typeof(string), typeof(ColumnLayout), new PropertyMetadata(null));




		public static SortColumnList GetDefaultSortColumns(DependencyObject obj)
		{
			var list = (SortColumnList)obj.GetValue(DefaultSortColumnsProperty);
			if (list == null)
			{
				list = new SortColumnList();
				obj.SetValue(DefaultSortColumnsProperty, list);
			}
			return (SortColumnList)obj.GetValue(DefaultSortColumnsProperty);
		}

		public static void SetDefaultSortColumns(DependencyObject obj, SortColumnList value)
		{
			obj.SetValue(DefaultSortColumnsProperty, value);
		}

		public static readonly DependencyProperty DefaultSortColumnsProperty =
			DependencyProperty.RegisterAttached("DefaultSortColumns", typeof(SortColumnList), typeof(ColumnLayout));




	}
}
