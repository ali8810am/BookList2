using System;
using View.Model;
using Microsoft.AspNetCore.Mvc;
using View.Model;

namespace View.Controller
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
