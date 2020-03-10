using System;

namespace Doozr.Common.Data
{
	public interface IEntity<T>
	{
		T Id{ get; set; }
	}

	public interface IEntity : IEntity<Guid> { }
}
