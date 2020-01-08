using System;

namespace Doozr.Common.Utilities.ExceptionHandling.Preconditions
{
	public static class Requires
	{
		public static void NotNull(object argument, string argumentName)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
	}
}
