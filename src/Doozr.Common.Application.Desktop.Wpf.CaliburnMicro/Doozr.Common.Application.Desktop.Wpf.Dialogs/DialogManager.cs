using Caliburn.Micro;
using Doozr.Common.Application.Desktop.Wpf.Dialogs.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doozr.Common.Application.Desktop.Wpf.Dialogs
{
	public class DialogManager : IDialogManager
	{
		private readonly IWindowManager windowManager;
		private readonly DialogViewModel.Factory dialogViewModelFactory;
		private readonly ConfirmDialogViewModel.Factory confirmDialogViewModelFactory;

		public DialogManager(
			IWindowManager windowManager,
			DialogViewModel.Factory dialogViewModelFactory,
			ConfirmDialogViewModel.Factory confirmDialogViewModelFactory)
		{
			this.windowManager = windowManager;
			this.dialogViewModelFactory = dialogViewModelFactory;
			this.confirmDialogViewModelFactory = confirmDialogViewModelFactory;
		}

		public Task<bool?> ShowConfirmDialogAsync(string message)
		{
			var confirmDialogViewModel = confirmDialogViewModelFactory(message);
			return windowManager.ShowDialogAsync(confirmDialogViewModel);
		}

		public Task<bool?> ShowDialogAsync(object viewModel, object context = null, IDictionary<string, object> settings = null)
		{
			var dialogViewModel = dialogViewModelFactory(viewModel);
			return windowManager.ShowDialogAsync(dialogViewModel, context, settings);
		}
	}
}
