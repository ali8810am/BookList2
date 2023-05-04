using View.Model;
using View.Services.Base;

namespace View.Contracts
{
    public interface IBookService
    {
        Task<List<BookVm>> GetBooks();
        Task<BookVm> GetBook(int id);
        Task<Response<int>> CreateBook(CreateBookVm book);
        Task<Response<int>> UpdateBook(int id, CreateBookVm book);
        Task DeleteBook(int id);
    }
}
