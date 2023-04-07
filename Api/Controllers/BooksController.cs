using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public async Task<ActionResult<IList<BookDto>>> Get()
        {
            var books = await _unitOfWork.Books.GetAll();

            return Ok(_mapper.Map<IList<BookDto>>(books));
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> Get(int id)
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
            var book =await _unitOfWork.Books.Get(b => b.Id == id);
            if (book==null)
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
