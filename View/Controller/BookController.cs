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
      
        public IActionResult Index()
        {
            return View();
        }

    
    }
}
