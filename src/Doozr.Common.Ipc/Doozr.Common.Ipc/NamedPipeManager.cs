using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Doozr.Common.Ipc
{
	public class NamedPipeManager : INamedPipeManager
	{
		public string[] GetNamedPipes()
		{
			return getNamedPipes().ToArray();
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		struct WIN32_FIND_DATA
		{
			public uint dwFileAttributes;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
			public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
			public uint nFileSizeHigh;
			public uint nFileSizeLow;
			public uint dwReserved0;
			public uint dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
			public string cAlternateFileName;
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);


		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA
			 lpFindFileData);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool FindClose(IntPtr hFindFile);

		private List<string> getNamedPipes()
		{
			List<string> liNamedPipes = new List<string>();
			WIN32_FIND_DATA lpFindFileData;

			var ptr = FindFirstFile(@"\\.\pipe\*", out lpFindFileData);
			liNamedPipes.Add(lpFindFileData.cFileName);
			while (FindNextFile(ptr, out lpFindFileData))
			{
				liNamedPipes.Add(lpFindFileData.cFileName.ToLower());
			}
			FindClose(ptr);

			liNamedPipes.Sort();

			return liNamedPipes;
		}
	}
}
