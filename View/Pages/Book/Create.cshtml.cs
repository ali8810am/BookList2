using View.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Microsoft.AspNetCore.Authorization;
using View.ConstantParameters;
using View.Contracts;
using View.Model;

namespace View.Pages.Book
{
    [Authorize(Roles = $"{UserRoles.Employee},{UserRoles.Admin}")]
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
            Book = new CreateBookVm();
        }
        public async Task<IActionResult> OnPost()
        {
           
            if (ModelState.IsValid)
            {
               var response= await _bookService.CreateBook(Book);
               if (response.Success)
                   return LocalRedirect("/Book/Index");
               ModelState.AddModelError("", response.ValidationErrors);
               return Page();
            }
            else
            {
                return Page();
            }
        }

    }
}
