using System;

namespace Doozr.Common.Utilities.ExceptionHandling.Preconditions
{
	public static class Requires
	{
		public static void NotNull(object argument, string argumentName)
		{Oops
			if (argument == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
	}
}
