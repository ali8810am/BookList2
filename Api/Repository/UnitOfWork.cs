using Api.Data;
using Api.IRepository;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Repository
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

        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }
    }
}
