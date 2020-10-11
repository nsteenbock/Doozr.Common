using System;
using System.Collections.Generic;
using System.Text;

namespace Doozr.Common.Ipc
{
	public class ByteArrayWriter
	{
		private readonly List<byte> bytes = new List<byte>();

		public void WriteGuid(Guid guid)
		{
			bytes.AddRange(guid.ToByteArray());
		}

		public void WriteInt32(Int32 value)
		{
			bytes.AddRange(BitConverter.GetBytes(value));
		}

		public void WriteString(string value)
		{
			var stringBytes = Encoding.UTF8.GetBytes(value);
			WriteInt32(stringBytes.Length);
			bytes.AddRange(stringBytes);
		}

		public void WriteBytes(byte[] bytesToWrite)
		{
			WriteInt32(bytesToWrite.Length);
			bytes.AddRange(bytesToWrite);
		}

		public byte[] Bytes => bytes.ToArray();
	}
}
