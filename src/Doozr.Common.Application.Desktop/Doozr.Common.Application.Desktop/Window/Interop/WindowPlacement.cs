using System;
using System.Runtime.InteropServices;

namespace Doozr.Common.Application.Desktop.Window.Interop
{
	[Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
        public int length;
        public int flags;
        public int showCmd;
        public Point minPosition;
        public Point maxPosition;
        public Rect normalPosition;
    }
}
