using Microsoft.EntityFrameworkCore;

namespace Doozr.Common.Data.EntityFramework
{
	public abstract class UnitOfWork : IUnitOfWork
	{
		protected readonly DbContext Context;

		public UnitOfWork(DbContext context)
		{
			this.Context = context;
		}

		public int Complete()
		{
			return Context.SaveChanges();
		}

		public void Dispose()
		{
			Context.Dispose();
		}
	}
}
