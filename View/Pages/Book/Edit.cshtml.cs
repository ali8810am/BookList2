using AutoMapper;
using View.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;
using View.Model;
using View.Services.Base;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using View.ConstantParameters;

namespace View.Pages.Booklist
{
    [Authorize(Roles = $"{UserRoles.Employee},{UserRoles.Admin}")]
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
               var response= await _bookService.UpdateBook(BookId, Book);
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
