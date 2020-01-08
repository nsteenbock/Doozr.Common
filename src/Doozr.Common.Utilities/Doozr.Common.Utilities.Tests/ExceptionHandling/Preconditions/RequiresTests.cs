using Doozr.Common.Utilities.ExceptionHandling.Preconditions;
using System;
using Xunit;

namespace Doozr.Common.Utilities.Tests.ExceptionHandling.Preconditions
{
	public class RequiresTests
	{
		[Fact]
		public void NotNull_ArgumentIsNullAndArgumentNameValid()
		{
			Assert.ThrowsAny<ArgumentNullException>(() => Requires.NotNull(null, "someArgumentName"));
		}
	}
}
