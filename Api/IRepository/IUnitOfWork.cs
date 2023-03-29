using Api.Data;

namespace Api.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<Book> Books { get; }
    }
}
