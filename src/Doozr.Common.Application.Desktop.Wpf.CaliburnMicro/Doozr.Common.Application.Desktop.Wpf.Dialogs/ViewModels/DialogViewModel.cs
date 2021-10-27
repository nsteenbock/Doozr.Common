using Caliburn.Micro;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Doozr.Common.Application.Desktop.Wpf.Dialogs.ViewModels
{
	public class DialogViewModel: Screen
	{
		public delegate DialogViewModel Factory(object innerViewModel);

		public DialogViewModel(object innerViewModel)
		{
			InnerViewModel = innerViewModel;

			if (innerViewModel is INotifyPropertyChanged)
			{
				(innerViewModel as INotifyPropertyChanged).PropertyChanged += InnerViewModelPropertyChanged;
			}
		}

		private void InnerViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(CanCancel))
			{
				this.CanCancel = (sender as ICancellable).CanCancel;
				NotifyOfPropertyChange(nameof(CanCancel));
			}
		}

		public object InnerViewModel { get; }

		public void Ok()
		{
			this.TryCloseAsync(true);
		}

		public async void Cancel()
		{
			await (InnerViewModel as ICancellable)?.Cancel();
		}

		public bool CanCancel { get; private set; } = false;

		protected override async Task OnActivateAsync(CancellationToken cancellationToken)
		{
			if (InnerViewModel is IDialogViewModel)
			{
				await (InnerViewModel as IDialogViewModel)?.ActivateAsync();
			}
			await base.OnActivateAsync(cancellationToken);
		}
	}
}
