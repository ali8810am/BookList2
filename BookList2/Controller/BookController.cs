using System;
using BookList2.Model;
using Microsoft.AspNetCore.Mvc;

namespace BookList2.Controller
{
    [Route("api/Book")]
    [ApiController]
    public class BookController : Microsoft.AspNetCore.Mvc.Controller
    {
        ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new {data=_context.Books.ToString() });
        }
    }
}
