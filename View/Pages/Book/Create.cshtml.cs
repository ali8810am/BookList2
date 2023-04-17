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


        public CreateModel(IBookService bookService)
        {
            _bookService = bookService;
        }
        [BindProperty]
        public CreateBookVm Book { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                await _bookService.CreateBook(Book);
                return LocalRedirect("/Book/Index");
            }
            else
            {
                return Page();
            }
        }

    }
}
