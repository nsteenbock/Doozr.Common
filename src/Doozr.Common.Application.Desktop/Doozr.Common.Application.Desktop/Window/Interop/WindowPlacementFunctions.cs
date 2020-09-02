using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Doozr.Common.Application.Desktop.Window.Interop
{
	public static class WindowPlacementFunctions
	{
		private static Encoding encoding = new UTF8Encoding();
		private static XmlSerializer serializer = new XmlSerializer(typeof(WindowPlacement));

		[DllImport("user32.dll")]
		private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WindowPlacement lpwndpl);

		[DllImport("user32.dll")]
		private static extern bool GetWindowPlacement(IntPtr hWnd, out WindowPlacement lpwndpl);

		private const int SW_SHOWNORMAL = 1;
		private const int SW_SHOWMINIMIZED = 2;

		public static void SetPlacement(IntPtr windowHandle, string placementXml)
		{
			if (string.IsNullOrEmpty(placementXml))
			{
				return;
			}

			WindowPlacement placement;
			byte[] xmlBytes = encoding.GetBytes(placementXml);

			using (MemoryStream memoryStream = new MemoryStream(xmlBytes))
			{
				placement = (WindowPlacement)serializer.Deserialize(memoryStream);
			}

			placement.length = Marshal.SizeOf(typeof(WindowPlacement));
			placement.flags = 0;
			placement.showCmd = (placement.showCmd == SW_SHOWMINIMIZED ? SW_SHOWNORMAL : placement.showCmd);
			SetWindowPlacement(windowHandle, ref placement);
		}

		public static string GetPlacement(IntPtr windowHandle)
		{
			WindowPlacement placement = new WindowPlacement();
			GetWindowPlacement(windowHandle, out placement);

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
				{
					serializer.Serialize(xmlTextWriter, placement);
					byte[] xmlBytes = memoryStream.ToArray();
					return encoding.GetString(xmlBytes);
				}
			}
		}
	}
}
