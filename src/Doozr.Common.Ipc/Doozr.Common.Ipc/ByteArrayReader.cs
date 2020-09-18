using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Doozr.Common.Ipc
{
	public class ByteArrayReader
	{
		private readonly byte[] bytes;

		private int position = 0;

		public ByteArrayReader(byte[] bytes)
		{
			this.bytes = bytes;
		}

		public Guid ReadGuid()
		{
			Guid result = new Guid(new Span<byte>(bytes, position, 16).ToArray());
			position += 16;
			return result;
		}

		public string ReadString()
		{
			var length = ReadInt32();
			var result = Encoding.UTF8.GetString(new Span<byte>(bytes, position, length).ToArray());
			position += length;
			return result;
		}

		public Int32 ReadInt32()
		{
			var result  = BitConverter.ToInt32(bytes, position);
			position += sizeof(Int32);
			return result;
		}

		public byte[] ReadBytes()
		{
			var length = ReadInt32();
			var result = new Span<byte>(bytes, position, length).ToArray();
			position += length;
			return result;
		}
	}
}
