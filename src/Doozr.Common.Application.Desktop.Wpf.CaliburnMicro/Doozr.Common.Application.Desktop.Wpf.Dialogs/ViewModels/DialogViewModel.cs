using Caliburn.Micro;

namespace Doozr.Common.Application.Desktop.Wpf.Dialogs.ViewModels
{
	public class DialogViewModel: Screen
	{
		public delegate DialogViewModel Factory(object innerViewModel);

		public DialogViewModel(object innerViewModel)
		{
			InnerViewModel = innerViewModel;
		} 

		public object InnerViewModel { get; }

		public void Ok()
		{
			this.TryCloseAsync(true);
		}
	}
}
