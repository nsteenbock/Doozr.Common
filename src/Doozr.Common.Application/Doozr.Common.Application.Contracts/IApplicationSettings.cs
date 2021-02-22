namespace Doozr.Common.Application
{
	public interface IApplicationSettings
	{
		T Get<T>();

		void Save();
	}
}
