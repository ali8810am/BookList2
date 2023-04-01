
using BookList.Domain.IRepository;


namespace BookList.Domain.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IBookRepository BookRepository { get; }
        Task Save();
    }
}
