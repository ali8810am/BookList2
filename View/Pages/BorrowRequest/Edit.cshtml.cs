using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;
using View.Model;

namespace View.Pages.BorrowRequest
{
    public class EditModel : PageModel
    {
        private readonly IBorrowRequestService _borrowRequestService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public EditModel(IBorrowRequestService borrowRequestService, IBookService bookService, IUserService userService, IMapper mapper)
        {
            _borrowRequestService = borrowRequestService;
            _bookService = bookService;
            _userService = userService;
            _mapper = mapper;
        }

        [BindProperty]
        public int RequsetId { get; set; }
        [BindProperty]
        public BorrowRequestVm Request { get; set; }

        public CreateBookVm BookVm { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            RequsetId = id;
            Request = await _borrowRequestService.GetBorrowRequest(id);
            var book = await _bookService.GetBook(Request.BookId);
            BookVm = _mapper.Map<CreateBookVm>(book);
            if (Request == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            Request.UpdatedBy = _userService.GetCurrentUserName();
            if (ModelState.IsValid)
            {
                var response = await _borrowRequestService.UpdateBorrowRequest(Request);
                if (response.Success)
                {
                    return LocalRedirect("");
                }
                ModelState.AddModelError("",response.ValidationErrors);
                return Page();
            }
            return Page();
        }
    }
}
