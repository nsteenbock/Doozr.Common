using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doozr.Common.Application.Desktop.Wpf.Dialogs
{
	public interface IDialogManager
	{
		Task<bool?> ShowDialogAsync(object viewModel, object context = null, IDictionary<string, object> settings = null);

		Task<bool?> ShowConfirmDialogAsync(string message);
	}
}
