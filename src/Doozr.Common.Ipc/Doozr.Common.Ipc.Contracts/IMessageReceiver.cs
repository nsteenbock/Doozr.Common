using System;
using System.Collections.Generic;
using System.Text;

namespace Doozr.Common.Ipc
{
	public interface IMessageReceiver
	{
		event Action<byte[], Action<byte[]>> OnMessageReceived;
	}
}
