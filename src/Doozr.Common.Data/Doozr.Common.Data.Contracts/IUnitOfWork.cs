using System;

namespace Doozr.Common.Data
{
	public interface IUnitOfWork: IDisposable
	{
		int Complete();
	}
}
