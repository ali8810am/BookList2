using AutoMapper;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Services
{
    public class BookService:IBookService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IClient _client;

        public BookService(ILocalStorageService storageService, IMapper mapper, IClient client)
        {
            _storageService = storageService;
            _mapper = mapper;
            _client = client;
        }
        public async Task<List<BookVm>> GetBooks()
        {
           var books=await _client.BooksAllAsync();
           return _mapper.Map<List<BookVm>>(books);
        }

        public async Task<BookVm> GetBook(int id)
        {
            var book = await _client.BooksGETAsync(id);
            return _mapper.Map<BookVm>(book);
        }

        public async Task<Response<int>> CreateBook(CreateBookVm book)
        {
            var bookDto = _mapper.Map<CreateBookDto>(book);
            await _client.BooksPOSTAsync(bookDto);
            return new Response<int>
            {
            };
        }

        public async Task<Response<int>> UpdateBook(int id, CreateBookVm book)
        {
            var bookDto = _mapper.Map<CreateBookDto>(book);
            await _client.BooksPUTAsync(id, bookDto);
            return new Response<int> { };
        }

        public async Task<Response<int>> DeleteBook(int id)
        {
            await _client.BooksDELETEAsync(id);
            return new Response<int> { };
        }
    }
}
