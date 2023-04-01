
using BookList.Domain.IRepository;
using BookList.Persistance.Data;
using BookList.Persistance.Data;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookList.Persistance.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<Book> _books;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<Book> Books => _books ??= new GenericRepository<Book>(_context);

        public IBookRepository BookRepository { get; }

        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }
    }
}
