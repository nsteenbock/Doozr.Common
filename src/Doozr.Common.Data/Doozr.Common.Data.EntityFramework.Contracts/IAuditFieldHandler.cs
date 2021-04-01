using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Doozr.Common.Data.EntityFramework
{
	public interface IAuditFieldHandler
	{
		void HandleAuditFileds(ChangeTracker changeTracker);
	}
}
