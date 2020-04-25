using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Doozr.Common.Data.EntityFramework
{
	public abstract class UnitOfWork : IUnitOfWork
	{
		protected readonly DbContext Context;

		private readonly object lockObject = new object();

		private Dictionary<Type, object> repositories = new Dictionary<Type, object>();

		private readonly Type[] repositoryConstructorArgTypes = new Type[] { typeof(DbContext) };

		private readonly object[] repositoryConstructorArgs;

		protected T GetRepository<T>() where T: class
		{
			var repositoryType = typeof(T);

			if (!repositories.ContainsKey(repositoryType))
			{
				lock(lockObject)
				{
					if (!repositories.ContainsKey(repositoryType))
					{
						var constructor = repositoryType.GetConstructor(repositoryConstructorArgTypes);
						repositories.Add(repositoryType, constructor.Invoke(repositoryConstructorArgs));
					}
				}
			}

			return (T)repositories[repositoryType];
		}

		public UnitOfWork(DbContext context)
		{
			this.Context = context;
			repositoryConstructorArgs = new object[] { context };
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
