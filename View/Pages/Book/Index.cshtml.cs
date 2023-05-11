using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;
using View.Model;

namespace View.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IBookService _bookService;

        public IndexModel(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IEnumerable<BookVm>? Books { get; set; }
        public async Task OnGet()
        {
          Books=await _bookService.GetBooks();
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
