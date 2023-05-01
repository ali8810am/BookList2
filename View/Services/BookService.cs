using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Services
{
    public class BookService:BaseHttpService,IBookService
    {
        private readonly ILocalStorageService _storageService;
        private readonly IMapper _mapper;
        private readonly IClient _client;

        public BookService(ILocalStorageService localStorageService, IClient client, ILocalStorageService storageService, IMapper mapper) : base(localStorageService, client)
        {
            _storageService = storageService;
            _mapper = mapper;
            _client = client;
        }
        public async Task<List<BookVm>> GetBooks()
        {
           var books=await _client.GetAllBooksAsync(null,null,null);
           return _mapper.Map<List<BookVm>>(books);
        }

        public async Task<BookVm> GetBook(int id)
        {
            var book = await _client.BooksGETAsync(id,null);
            return _mapper.Map<BookVm>(book);
        }
        [Authorize]
        public async Task<Response<int>> CreateBook(CreateBookVm book)
        {
            var bookDto = _mapper.Map<CreateBookDto>(book);
            AddBearerToken();
            await _client.BooksPOSTAsync(bookDto);
            return new Response<int>
            {
            };
        }

        public async Task<Response<int>> UpdateBook(int id, CreateBookVm book)
        {
            var bookDto = _mapper.Map<CreateBookDto>(book);
            AddBearerToken();
            await _client.BooksPUTAsync(id, bookDto);
            return new Response<int> { };
        }

        public async Task<Response<int>> DeleteBook(int id)
        {
            await _client.BooksDELETEAsync(id);
            AddBearerToken();
            return new Response<int> { };
        }
    }
}
