using AutoMapper;
using View.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;
using View.Model;

namespace View.Pages.Booklist
{
    public class EditModel : PageModel
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public EditModel(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        [BindProperty]
        public int BookId { get; set; }
        [BindProperty]
        public CreateBookVm Book { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            BookId = id;
            var book = await _bookService.GetBook(id);
            if (book == null)
                return NotFound();
            Book = _mapper.Map<CreateBookVm>(book);
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                await _bookService.UpdateBook(BookId, Book);
                return LocalRedirect("/Book/Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
