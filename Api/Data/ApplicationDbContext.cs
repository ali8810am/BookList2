using System.Security.Claims;
using Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace Api.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApiUser>
    {
        private readonly IUserService _userService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IUserService userService)
            : base(options)
        {
            _userService = userService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BorrowAllocation>().HasOne<Book>(a=>a.Book).WithMany(b => b.BorrowAllocations).HasForeignKey("BookId")
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<BorrowAllocation>().HasOne<Employee>(a=>a.Employee).WithMany(e => e.BorrowAllocations)
                .HasForeignKey("EmployeeId")
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<BorrowAllocation>().HasOne<Customer>(a=>a.Customer).WithMany(c => c.BorrowAllocations)
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<BorrowRequest>().HasOne<Customer>(r=>r.Customer).WithMany(c => c.BorrowRequests)
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<BorrowRequest>().HasOne<Book>(r=>r.Book).WithMany(b => b.BorrowRequests).HasForeignKey("BookId")
                .OnDelete(DeleteBehavior.Restrict);
        }

      
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseDomainObject && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseDomainObject)entityEntry.Entity).UpdatedDateTime = DateTime.Now;
                ((BaseDomainObject)entityEntry.Entity).UpdatedBy = _userService.GetCurrentUserName();

                if (entityEntry.State != EntityState.Added) continue;
                ((BaseDomainObject)entityEntry.Entity).CreatedDateTime = DateTime.Now;
                ((BaseDomainObject)entityEntry.Entity).CreatedBy = _userService.GetCurrentUserName();
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowAllocation> BorrowAllocations { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }


    }
}
