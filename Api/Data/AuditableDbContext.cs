using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Api.Data
{
    public class AuditableDbContext : IdentityDbContext<ApiUser>
    {
        public AuditableDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual async Task<int> SaveChangesAsync(string? username)
        {
            username ??= "system";
                foreach (var entry in base.ChangeTracker.Entries<BaseDomainObject>()
                             .Where(q=>q.State==EntityState.Added||q.State == EntityState.Modified))
            {
                entry.Entity.UpdatedDateTime = DateTime.Now;
                entry.Entity.UpdatedBy = username;
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDateTime = DateTime.Now;
                    entry.Entity.CreatedBy = username;
                }
            }
            var result = await base.SaveChangesAsync();
            return result;
        }

    }
}
