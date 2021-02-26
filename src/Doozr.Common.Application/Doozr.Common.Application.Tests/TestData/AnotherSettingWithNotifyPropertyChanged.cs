using System.ComponentModel;

namespace Doozr.Common.Application.Tests.TestData
{
	public class AnotherSettingWithNotifyPropertyChanged : INotifyPropertyChanged
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
