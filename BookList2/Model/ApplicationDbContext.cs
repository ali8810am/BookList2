using Microsoft.EntityFrameworkCore;

namespace BookList2.Model
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option)
            : base(option)
        {
            
        }
        public DbSet<Book> Books { get; set; }
    }
}
