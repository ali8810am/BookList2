using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace Api.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
        {
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

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowAllocation> BorrowAllocations { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }


    }
}
