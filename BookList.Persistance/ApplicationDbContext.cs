<<<<<<< HEAD:BookList.Persistance/ApplicationDbContext.cs
﻿using BookList.Persistance.Data;
=======
﻿using System.Security.Claims;
using Api.Services;
using Microsoft.AspNetCore.Identity;
>>>>>>> e31b9b8125159a0d7956dae5eec28b0187a1cf00:Api/Data/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookList.Persistance.Data
{
    public class ApplicationDbContext : AuditableDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BorrowAllocation>().HasOne<Book>(a => a.Book).WithMany(b => b.BorrowAllocations).HasForeignKey("BookId")
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<BorrowAllocation>().HasOne<Employee>(a => a.Employee).WithMany(e => e.BorrowAllocations)
                .HasForeignKey("EmployeeId")
                .OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<BorrowAllocation>().HasOne<Customer>(a => a.Customer).WithMany(c => c.BorrowAllocations)
            //    .HasForeignKey("CustomerId")
            //    .OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<BorrowRequest>().HasOne<Customer>(r => r.Customer).WithMany(c => c.BorrowRequests)
            //    .HasForeignKey("CustomerId")
            //    .OnDelete(DeleteBehavior.Cascade);
            //builder.Entity<BorrowRequest>().HasOne<Book>(r => r.Book).WithMany(b => b.BorrowRequests).HasForeignKey("BookId")
            //    .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowAllocation> BorrowAllocations { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }


    }
}
