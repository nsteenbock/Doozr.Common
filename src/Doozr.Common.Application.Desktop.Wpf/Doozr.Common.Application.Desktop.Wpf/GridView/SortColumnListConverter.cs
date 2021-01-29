using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Doozr.Common.Application.Desktop.Wpf.GridView
{
	public class SortColumnListConverter: TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string sortColumnsDescription)
			{
				var columns = sortColumnsDescription.Split(',');
				var result = new SortColumnList(columns.Select(x => GetSortColumnFromDescription(x)).ToArray());
				return result;
			}
			return base.ConvertFrom(context, culture, value);
		}

		private static readonly string[] descendingValues = new[] { "d", "desc", "descending", "D", "Desc", "Descending" };

		private SortColumn GetSortColumnFromDescription(string sortColumnDescription)
		{
			var result = new SortColumn();
			var match = Regex.Match(sortColumnDescription, 
				@"^(?<id>[^\(]*)(\((?<order>(Ascending|Descending|A|D|Asc|Desc))\))?$",
				RegexOptions.IgnoreCase);
			result.Id = match.Groups["id"].Value;
			result.SortDirection = descendingValues.Contains(match.Groups["order"].Value) ? ListSortDirection.Descending : ListSortDirection.Ascending;
			return result;
		}
	}
}
