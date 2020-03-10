using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doozr.Common.Data.EntityFramework
{
	public class Repository<TId, TEntity> : IRepository<TId, TEntity> where TEntity: class, IEntity<TId>
	{
		protected readonly DbContext Context;

		public Repository(DbContext context)
		{
			this.Context = context;
		}

		public void Add(TEntity entity)
		{
			Context.Set<TEntity>().Add(entity);
		}

		public void AddRange(IEnumerable<TEntity> entities)
		{
			Context.Set<TEntity>().AddRange(entities);
		}

		public IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
		{
			return Context.Set<TEntity>().Where(predicate);
		}

		public TEntity Get(TId id)
		{
			return Context.Set<TEntity>().Find(id);
		}

		public IEnumerable<TEntity> GetAll()
		{
			return Context.Set<TEntity>().ToArray();
		}

		public void Remove(TEntity entity)
		{
			Context.Set<TEntity>().Remove(entity);
		}

		public void RemoveRange(IEnumerable<TEntity> entities)
		{
			Context.Set<TEntity>().RemoveRange(entities);
		}
	}

	public class Repository<TEntity> : Repository<Guid, TEntity> where TEntity : class, IEntity
	{
		public Repository(DbContext context) : base(context)
		{
		}
	}
}
