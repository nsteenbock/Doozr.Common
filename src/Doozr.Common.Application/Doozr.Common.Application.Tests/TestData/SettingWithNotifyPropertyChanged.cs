using System.ComponentModel;

namespace Doozr.Common.Application.Tests.TestData
{
	public class SettingWithNotifyPropertyChanged : INotifyPropertyChanged
	{
		private int value;

		public event PropertyChangedEventHandler PropertyChanged;

		public int Value
		{
			get => value;
			set
			{
				this.value = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}
	}
}
