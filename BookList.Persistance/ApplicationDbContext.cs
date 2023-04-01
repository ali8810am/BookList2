using BookList.Persistance.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookList.Persistance.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
        {
        }

        public DbSet<Book> Books { get; set; }


    }
}
