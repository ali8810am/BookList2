using View.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using View.Contracts;
using View.Model;

namespace View.Pages.Book
{
    public class CreateModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(IBookService bookService, IHttpContextAccessor httpContextAccessor)
        {
            _bookService = bookService;
            _httpContextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public CreateBookVm Book { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var name = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (ModelState.IsValid)
            {
                await _bookService.CreateBook(Book);
                return LocalRedirect("Index");
            }
            else
            {
                return Page();
            }
        }

    }
}
