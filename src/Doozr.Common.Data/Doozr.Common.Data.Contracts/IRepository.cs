using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Doozr.Common.Data
{
	public interface IRepository<TId, TEntity> where TEntity: class, IEntity<TId>
	{
		TEntity Get(TId id);
		IEnumerable<TEntity> GetAll();
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> entities);

		void Remove(TEntity entity);
		void RemoveRange(IEnumerable<TEntity> entities);
	}

	public interface IRepository<TEntity> : IRepository<Guid, TEntity> where TEntity : class, IEntity { }
}
