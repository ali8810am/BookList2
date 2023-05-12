using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.ConstantParameters;
using View.Contracts;
using View.Model;

namespace View.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IHttpContextAccessor _contextAccessor;

        public IndexModel(IBookService bookService, IHttpContextAccessor contextAccessor)
        {
            _bookService = bookService;
            _contextAccessor = contextAccessor;
        }

        public IEnumerable<BookVm>? Books { get; set; }
        public async Task OnGet()
        {
            if (_contextAccessor.HttpContext.User.IsInRole(UserRoles.Customer))
            {
                Books=await _bookService.GetBooksInLibrary();
            }
            else
            {
                Books = await _bookService.GetBooks();
            }
        }

        public async Task<IActionResult> OnGetDelete(int id)
        {
            var book =await _bookService.GetBook(id);
            if (book == null)
                return NotFound();
            await _bookService.DeleteBook(id);
            return RedirectToPage("/Book/Index");
        }
    }
}
