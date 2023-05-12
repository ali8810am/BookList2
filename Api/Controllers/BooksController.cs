using Api.Data;
using Api.Exceptions;
using Api.IRepository;
using Api.Models;
using Api.Models.QueryParameter;
using Api.Responses;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Api.ConstantParameters;
using Api.Models.Validators.Book;
using X.PagedList;
using Marvin.Cache.Headers;

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

        [HttpGet]
        [Route("GetAllBooks")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<ActionResult<IList<BookDto>>> GetAll([FromQuery] QueryParameter? parameters)
        {
            var books = await _unitOfWork.Books.GetAll(parameters.RequestParameters, null, parameters.includes);
            var bookDto=new List<BookDto>();
            bookDto = _mapper.Map<List<BookDto>>(books);
                return Ok(bookDto);
        }
        [HttpGet]
        [Route("GetFilteredBooks")]
        public async Task<ActionResult<IList<BookDto>>> GetAll([FromQuery] BookQueryParameter parameter)
        {

            
                var booksQuery = _unitOfWork.Books.GetFiltered(parameter.includes);
                if (parameter.Name!=null)
                {
                    booksQuery = booksQuery
                        .Where(b => b.Name.ToLower().Contains(parameter.Name.ToLower()) ||
                                    string.IsNullOrWhiteSpace(parameter.Author));
                }
                if (parameter.Author != null)
                {
                    booksQuery = booksQuery
                        .Where(b => b.Author.ToLower().Contains(parameter.Author.ToLower()) ||
                                    string.IsNullOrWhiteSpace(parameter.Author));
                }
                if (parameter.IsInLibrary != null)
                {
                    booksQuery = booksQuery
                        .Where(b => b.IsInLibrary == parameter.IsInLibrary);
                }
                var filteredBooks = booksQuery.ToPagedList(parameter.RequestParameters.PageNumber,
                         parameter.RequestParameters.PageSize);
                var bookDto = new List<BookDto>();
                bookDto = _mapper.Map<List<BookDto>>(filteredBooks);
                return Ok(bookDto);
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<ActionResult<BookDto>> Get(int id, List<string>? includes = null)
        {
            var book = await _unitOfWork.Books.Get(b => b.Id == id);
            if (book == null)
                return NotFound();
            return Ok(_mapper.Map<BookDto>(book));
        }
        //[Authorize]
        // POST api/<BooksController>
        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Employee}")]
        public async Task<BaseCommandResponse> Post([FromBody] CreateBookDto bookDto)
        {
            var response = new BaseCommandResponse();
            var validator = new BookValidator();
            var validationResult = await validator.ValidateAsync(bookDto);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "CreationFailed";
            }
            else
            {


                var book = _mapper.Map<Book>(bookDto);
                await _unitOfWork.Books.Add(book);
                await _unitOfWork.Save();
            }
            return response;

        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Employee}")]
        public async Task<BaseCommandResponse> Put(int id, [FromBody] CreateBookDto bookDto)
        {
            var response = new BaseCommandResponse();
            var validator = new BookValidator();
            var validationResult = await validator.ValidateAsync(bookDto);
            if (id == 0)
                response.Errors.Add("Id must greater than 0");
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                response.Message = "CreationFailed";
            }
            else
            {
                var book = await _unitOfWork.Books.Get(b => b.Id == id);
                if (book == null)
                    throw new NotFoundException("book not found", id);
                _mapper.Map(bookDto, book);
                _unitOfWork.Books.Update(book);
                await _unitOfWork.Save();
            }

            return response;
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Employee}")]
        public async Task Delete(int id)
        {
            if (!await _unitOfWork.Books.Exist(b => b.Id == id))
                throw new NotFoundException("book not found", id);
            await _unitOfWork.Books.Delete(id);
            await _unitOfWork.Save();
        }
    }
}
