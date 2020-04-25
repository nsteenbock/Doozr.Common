using System;

namespace Doozr.Common.Data
{
	public abstract class Entity<T>: IEntity<T>
	{
		public T Id{ get; set; }
	}

	public abstract class Entity : Entity<Guid>, IEntity { }
}
