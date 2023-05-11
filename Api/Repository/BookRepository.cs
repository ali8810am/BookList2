using Api.Data;
using Api.IRepository;

namespace Api.Repository
{
    public class BookRepository:GenericRepository<Book>,IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
