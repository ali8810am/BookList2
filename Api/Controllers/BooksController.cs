using Api.Data;
using Api.IRepository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: api/<BooksController>
        [HttpGet]
        public async Task<ActionResult<IList<Book>>> Get()
        {
          var books= await _bookRepository.GetAll();
          return Ok(books);
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var books = await _bookRepository.Get(b=>b.Id==id);
            return Ok(books);
        }

        // POST api/<BooksController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
