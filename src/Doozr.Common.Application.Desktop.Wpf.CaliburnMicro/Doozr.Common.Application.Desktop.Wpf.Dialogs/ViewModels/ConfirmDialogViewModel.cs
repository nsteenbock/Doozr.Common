using Caliburn.Micro;

namespace Doozr.Common.Application.Desktop.Wpf.Dialogs.ViewModels
{
	public class ConfirmDialogViewModel: Screen
	{
		public string Message { get; }

		public delegate ConfirmDialogViewModel Factory(string message);

		public ConfirmDialogViewModel(string message)
		{
			Message = message;
		}

		public void Ok()
		{
			this.TryCloseAsync(true);
		}
	}
}
