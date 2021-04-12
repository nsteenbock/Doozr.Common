namespace Doozr.Common.Isolation.Io
{
	internal static class ConversionExtensions
	{
		public static FileInfo ToIsolationFileInfo(this System.IO.FileInfo systemIoFileInfo)
		{
			return new FileInfo
			{
				FullName = systemIoFileInfo.FullName,
				Name = systemIoFileInfo.Name,
				Length = systemIoFileInfo.Length
			};
		}
	}
}
