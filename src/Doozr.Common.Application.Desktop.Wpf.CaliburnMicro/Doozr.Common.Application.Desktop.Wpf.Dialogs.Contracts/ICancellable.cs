using System.Threading.Tasks;

namespace Doozr.Common.Application.Desktop.Wpf.Dialogs
{
	public interface ICancellable
	{
		Task Cancel();

		bool CanCancel{ get; }
	}
}
