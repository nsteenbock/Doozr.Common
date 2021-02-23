using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	public class ColumnLayoutViewModel: INotifyPropertyChanged
	{
		private readonly ColumnLayoutData columnLayoutData;
		private double actualGridWidth;

		public event PropertyChangedEventHandler PropertyChanged;

		public ColumnLayoutViewModel(ColumnLayoutData columnLayoutData)
		{
			this.columnLayoutData = columnLayoutData;
		}

		[AlsoNotifyFor(nameof(Columns))]
		public ColumnProperties[] AllColumns
		{
			get { return this.columnLayoutData.Columns; }
			set
			{
				foreach (var column in this.columnLayoutData.Columns)
				{
					(column as INotifyPropertyChanged).PropertyChanged -= ColumnLayoutViewModel_PropertyChanged;
				}
				this.columnLayoutData.Columns = value;
				foreach (var column in this.columnLayoutData.Columns)
				{
					(column as INotifyPropertyChanged).PropertyChanged += ColumnLayoutViewModel_PropertyChanged;
				}
			}
		}

		private void ColumnLayoutViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Width")
			{
				//if ((sender as ColumnProperties).Id != AutoSizeColumn)
				{
					RecalculateColumnWidths();
				}
			}
		}

		public void ChangeColumnSort(string id, bool extendSorting)
		{
			sortingsChanging = true;
			var result = new List<SortColumn>(SortColumns);
			try
			{
				var columnSort = SortColumns.Where(x => x.Id == id).FirstOrDefault();
				if (columnSort != null)
				{
					if (!extendSorting)
					{
						foreach (var column in SortColumns.Where(x => x.Id != id).ToArray())
						{
							result.Remove(column);
						}
					}
					columnSort.SortDirection = columnSort.SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending
						: ListSortDirection.Ascending;
				}
				else
				{
					if (!extendSorting)
					{
						result.Clear();
					}
					result.Add(new SortColumn { Id = id, SortDirection = ListSortDirection.Ascending });
				}
			}
			finally
			{
				sortingsChanging = false;
			}
			SortColumns = result.ToArray();
		}

		[AlsoNotifyFor(nameof(HasAutoSizeColumn))]
		public string AutoSizeColumn
		{
			get { return columnLayoutData.AutoSizeColumn; }
			set { columnLayoutData.AutoSizeColumn = value; }
		}

		public SortColumn[] SortColumns
		{
			get { return columnLayoutData.SortColumns; }
			set 
			{
				if (columnLayoutData.SortColumns != null)
				{
					foreach (var sortColumn in columnLayoutData.SortColumns)
						((INotifyPropertyChanged)sortColumn).PropertyChanged -= SortColumnsChanged;
				}
				columnLayoutData.SortColumns = value;
				if (columnLayoutData.SortColumns != null)
				{
					foreach(var sortColumn in columnLayoutData.SortColumns)
					((INotifyPropertyChanged)sortColumn).PropertyChanged += SortColumnsChanged;
				}
			}
		}

		bool sortingsChanging;

		private void SortColumnsChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!sortingsChanging)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SortColumns)));
			}
		}

		public bool HasAutoSizeColumn => !string.IsNullOrWhiteSpace(AutoSizeColumn);

		public IEnumerable<ColumnProperties> Columns => AllColumns.Where(x => !x.Hidden);

		internal void MoveVisibleColumn(int oldIndex, int newIndex)
		{
			var allColumnList = AllColumns.ToList();
			var columnList = Columns.ToList();
			var movedColumn = columnList[oldIndex];
			columnList.Remove(movedColumn);

			if (newIndex < oldIndex)
			{

				var subsequentColumn = columnList[newIndex];
				allColumnList.Remove(movedColumn);
				var subsequentColumnIndex = allColumnList.IndexOf(subsequentColumn);
				allColumnList.Insert(subsequentColumnIndex, movedColumn);
			}
			else
			{
				var precedingColumn = columnList[newIndex - 1];
				allColumnList.Remove(movedColumn);
				var precedingColumnIndex = allColumnList.IndexOf(precedingColumn);
				allColumnList.Insert(precedingColumnIndex + 1, movedColumn);
			}

			AllColumns = allColumnList.ToArray();
		}

		[DoNotCheckEquality]
		public double ActualGridWidth
		{
			get { return actualGridWidth; }
			set
			{
				actualGridWidth = value;
				RecalculateColumnWidths();
			}
		}

		public void RecalculateColumnWidths()
		{
			if (!string.IsNullOrEmpty(AutoSizeColumn))
			{
				var otherColumnsWidthSum = Columns.Where(x => x.Id != AutoSizeColumn).Sum(x => x.Width);
				var autoSizeColumn = Columns.Where(x => x.Id == AutoSizeColumn).Single();
				var sizeForAutoColumn = ActualGridWidth - otherColumnsWidthSum;
				if (sizeForAutoColumn > 0)
				{
					autoSizeColumn.Width = sizeForAutoColumn;
				}
			}
		}
	}
}
