using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	public class ColumnLayoutManager
	{
		private readonly ListView listView;
		private readonly System.Windows.Controls.GridView gridView;
		private readonly ColumnLayoutViewModel viewModel;

		public ColumnLayoutManager(ListView listView)
		{
			this.listView = listView;
			this.gridView = listView.View as System.Windows.Controls.GridView;

			this.viewModel = new ColumnLayoutViewModel(ColumnLayout.GetData(this.listView));
			this.listView.Loaded += ListView_Loaded;

			gridView.Columns.CollectionChanged += Columns_CollectionChanged;

			(this.viewModel as INotifyPropertyChanged).PropertyChanged += ViewModel_PropertyChanged;

			listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
		}

		private void ColumnHeader_Click(object sender, RoutedEventArgs e)
		{
			if (e.OriginalSource is GridViewColumnHeader columnHeader)
			{
				viewModel.ChangeColumnSort(Column.GetId(columnHeader.Column), Keyboard.Modifiers.HasFlag(ModifierKeys.Control));
				/*var sorting = viewModel.SortColumns.Where(x => x.Id == Column.GetId(columnHeader.Column)).FirstOrDefault();
				if (sorting != null)
				{
					if (sorting.SortDirection == ListSortDirection.Ascending)
					{
						sorting.SortDirection = ListSortDirection.Descending;
					}
					else
					{
						sorting.SortDirection = ListSortDirection.Ascending;
					}
				}*/
			}
		}

		private void ListView_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			var scrollViewer = listView.GetVisualChild<ScrollViewer>();
			var viewPortWidth = scrollViewer.ViewportWidth;
			viewModel.ActualGridWidth = viewPortWidth;
		}

		private void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			(viewModel as INotifyPropertyChanged).PropertyChanged -= ViewModel_PropertyChanged;
			try
			{
				if (e.Action == NotifyCollectionChangedAction.Move)
				{

					viewModel.MoveVisibleColumn(e.OldStartingIndex, e.NewStartingIndex);
				}
			}
			finally
			{
				(viewModel as INotifyPropertyChanged).PropertyChanged += ViewModel_PropertyChanged;
			}

		}

		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ColumnLayoutViewModel.Columns))
			{
				SetGridColumnsFromViewModel();
			}

			if (e.PropertyName == nameof(ColumnLayoutViewModel.HasAutoSizeColumn))
			{
				var scrollViewer = listView.GetVisualChild<ScrollViewer>();
				scrollViewer.HorizontalScrollBarVisibility = viewModel.HasAutoSizeColumn ? ScrollBarVisibility.Hidden
					: ScrollBarVisibility.Auto;
			}

			if (e.PropertyName == nameof(ColumnLayoutViewModel.SortColumns))
			{
				SynchronizeSortColumns();
			}
		}

		private void SetGridColumnsFromViewModel()
		{
			var availableColumns = ColumnLayout.GetAvailableColumns(listView);
			gridView.Columns.Clear();
			foreach (var column in viewModel.Columns)
			{
				gridView.Columns.Add(availableColumns.Single(x => Column.GetId(x) == column.Id));
			}
		}

		private void ListView_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			StoreAvailableColumns();
			SynchronizeColumns();
			SynchronizeAutoSizeProperties();
			ApplyBindings();
			SynchronizeSortColumns();
			var scrollViewer = listView.GetVisualChild<ScrollViewer>();
			scrollViewer.HorizontalScrollBarVisibility = viewModel.HasAutoSizeColumn ? ScrollBarVisibility.Hidden
					: ScrollBarVisibility.Auto;
			//scrollViewer.UpdateLayout();
			var viewPortWidth = scrollViewer.ViewportWidth;
			viewModel.ActualGridWidth = viewPortWidth;

			listView.SizeChanged += ListView_SizeChanged;
		}

		private void SynchronizeSortColumns()
		{
			listView.Items.SortDescriptions.Clear();

			Dictionary<GridViewColumn, GridViewColumnHeader> columnDict = new Dictionary<GridViewColumn, GridViewColumnHeader>();

			foreach (var column in gridView.Columns)
			{
				var header = listView.GetVisualChildren<GridViewColumnHeader>().Where(x => x.Column == column).Single();
				columnDict.Add(column, header);
				RemoveSortGlyph(header);
			}

			foreach (var sortColumn in viewModel.SortColumns)
			{
				var column = gridView.Columns.Where(x => Column.GetId(x) == sortColumn.Id).Single();
				var header = listView.GetVisualChildren<GridViewColumnHeader>().Where(x => x.Column == column).Single();
				AddSortGlyph(header, sortColumn.SortDirection, null);
				listView.Items.SortDescriptions.Add(new SortDescription(Column.GetPropertyName(column), sortColumn.SortDirection));
			}
		}

		private static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
			if (adornerLayer != null)
			{
				adornerLayer.Add(
					new SortGlyphAdorner(
						columnHeader,
						direction,
						sortGlyph
						));
			}
		}

		private static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
			if (adornerLayer != null)
			{
				Adorner[] adorners = adornerLayer.GetAdorners(columnHeader);
				if (adorners != null)
				{
					foreach (Adorner adorner in adorners)
					{
						if (adorner is SortGlyphAdorner)
							adornerLayer.Remove(adorner);
					}
				}
			}
		}

		private void SynchronizeAutoSizeProperties()
		{
			if (viewModel.AutoSizeColumn == null)
			{
				viewModel.AutoSizeColumn = ColumnLayout.GetDefaultAutosizeColumnId(listView);
			}
		}

		private void ApplyBindings()
		{
			var availableColumns = ColumnLayout.GetAvailableColumns(listView);

			foreach (var column in viewModel.AllColumns)
			{
				var availableColumn = availableColumns.Single(x => Column.GetId(x) == column.Id);
				Binding binding = new Binding("Width");
				binding.Source = column;
				binding.Mode = BindingMode.TwoWay;
				BindingOperations.SetBinding(availableColumn, GridViewColumn.WidthProperty, binding);
			}
		}

		private void SynchronizeColumns()
		{
			var allColumns = new List<ColumnProperties>();
			var availableColumns = ColumnLayout.GetAvailableColumns(listView);
			if (this.viewModel.AllColumns != null)
			{
				allColumns.AddRange(this.viewModel.AllColumns
					.Where(x => availableColumns.Any(y => Column.GetId(y) == x.Id)));
			}

			foreach (var missingColumn in availableColumns.Where(x => !allColumns.Any(y => Column.GetId(x) == y.Id)))
			{
				allColumns.Add(new ColumnProperties(Column.GetId(missingColumn))
				{
					Width = missingColumn.ActualWidth
				});
			}

			viewModel.AllColumns = allColumns.ToArray();

			if (this.viewModel.SortColumns == null)
			{
				viewModel.SortColumns = ColumnLayout.GetDefaultSortColumns(listView).ToArray();
			}
		}

		private void StoreAvailableColumns()
		{
			ColumnLayout.SetAvailableColumns(listView, gridView.Columns.ToList());
		}

		private ColumnProperties[] GetGridColumns()
		{
			return ColumnLayout.GetAvailableColumns(gridView).Select(x => new ColumnProperties(Column.GetId(x))
			{
				Hidden = Column.GetHiddenByDefault(x),
				Width = x.Width
			}).ToArray();
		}
	}
}
