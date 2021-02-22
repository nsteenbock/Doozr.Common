using System;

namespace Doozr.Common.Isolation.Time
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime Now => DateTime.Now;
	}
}
