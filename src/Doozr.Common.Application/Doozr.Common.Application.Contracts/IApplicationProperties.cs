namespace Doozr.Common.Application
{
	public interface IApplicationProperties
	{
		string ProcessName{ get; }

		string RootDirectory{ get; }

		string AppDataDirectory { get; }
	}
}
