using System;

namespace Doozr.Common.Isolation.Time
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }
	}
}
