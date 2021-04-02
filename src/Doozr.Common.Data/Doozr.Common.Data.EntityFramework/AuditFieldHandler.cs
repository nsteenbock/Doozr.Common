using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace Doozr.Common.Data.EntityFramework
{
	public class AuditFieldHandler : IAuditFieldHandler
	{
		public void HandleAuditFileds(ChangeTracker changeTracker)
		{
            foreach (var auditableEntry in changeTracker.Entries<IAuditable>())
            {
                if (auditableEntry.State == EntityState.Added ||
                    auditableEntry.State == EntityState.Modified)
                {
                    auditableEntry.Entity.Modified = DateTime.Now;
                    auditableEntry.Entity.Modifier = GetCurrentUsername();

                    if (auditableEntry.State == EntityState.Added)
                    {
                        auditableEntry.Entity.Created = DateTime.Now;
                        auditableEntry.Entity.Creator = GetCurrentUsername();
                    }
                }
            }
        }

		private string GetCurrentUsername()
		{
            return $"{Environment.UserDomainName}\\{Environment.UserName}";
		}
	}
}
