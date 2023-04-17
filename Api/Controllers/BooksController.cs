using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using Api.Models.QueryParameter;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BooksController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<BooksController>
        //[HttpGet]
        //public async Task<ActionResult<IList<BookDto>>> GetAll()
        //{
        //    var books = await _unitOfWork.Books.GetAll();

        //    return Ok(_mapper.Map<IList<BookDto>>(books));
        //}
        //[HttpGet]
        //public async Task<ActionResult<IList<BookDto>>> GetAll([FromQuery] RequestParameters? parameters )
        //{
        //    var books = await _unitOfWork.Books.GetAll(parameters);

        //    return Ok(_mapper.Map<IList<BookDto>>(books));
        //}
        [HttpGet]
        public async Task<ActionResult<IList<BookDto>>> GetAll([FromQuery] BookQueryParameter? parameter)
        {
            if (parameter.WantAll==true)
            {
                var allBooks = await _unitOfWork.Books.GetAll(null,parameter.includes);
                return Ok(_mapper.Map<IList<BookDto>>(allBooks));
            }

            if (parameter.Author != null||parameter.Name!=null)
            {
                var booksQuery= _unitOfWork.Books.GetFiltered(parameter.includes);
               var filteredBooks = booksQuery
                    .Where(b => b.Author.ToLower().Contains(parameter.Author.ToLower())  || string.IsNullOrWhiteSpace(parameter.Author))
                    .Where(b => b.Name.ToLower().Contains(parameter.Author.ToLower()) || string.IsNullOrWhiteSpace(parameter.Name))
                    .ToPagedList(parameter.RequestParameters.PageNumber,
                        parameter.RequestParameters.PageSize);
               return Ok(_mapper.Map<IList<BookDto>>(filteredBooks));
            }

            var books = await _unitOfWork.Books.GetAll(parameter.RequestParameters,null,parameter.includes);
            return Ok(_mapper.Map<IList<BookDto>>(books));
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> Get(int id, List<string> includes = null)
        {
            var book = await _unitOfWork.Books.Get(b => b.Id == id);
            return Ok(_mapper.Map<BookDto>(book));
        }
        [Authorize]
        // POST api/<BooksController>
        [HttpPost]
        public async Task Post([FromBody] CreateBookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            await _unitOfWork.Books.Add(book);
            await _unitOfWork.Save();
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] CreateBookDto bookDto)
        {
            var book = await _unitOfWork.Books.Get(b => b.Id == id);
            if (book == null)
                throw new NotFoundException("book not found", id);
            _mapper.Map(bookDto, book);
            _unitOfWork.Books.Update(book);
            await _unitOfWork.Save();
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (!await _unitOfWork.Books.Exist(b => b.Id == id))
                throw new NotFoundException("book not found", id);
            await _unitOfWork.Books.Delete(id);
            await _unitOfWork.Save();
        }
    }
}
