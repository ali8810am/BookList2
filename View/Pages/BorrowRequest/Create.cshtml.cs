using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;
using View.Model;

namespace View.Pages.BorrowRequest
{
    public class CreateModel : PageModel
    {
        private readonly IBorrowRequestService _borrowRequestService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateModel(IBorrowRequestService borrowRequestService, IBookService bookService, IUserService userService, IMapper mapper)
        {
            _borrowRequestService = borrowRequestService;
            _bookService = bookService;
            _userService = userService;
            _mapper = mapper;
        }
        [BindProperty]
        public CreateBorrowRequestVm Request { get; set; }= new CreateBorrowRequestVm();

        public CreateBookVm BookVm { get; set; }

        public async Task OnGet(int bookId)
        {
            var book = await _bookService.GetBook(bookId);
            BookVm=_mapper.Map<CreateBookVm>(book);
            Request.BookId=bookId;
        }

        public async Task<IActionResult> OnPost()
        {
                Request.DateRequested = DateTime.Now;
                Request.CreatedBy = _userService.GetCurrentUserName();
            if (ModelState.IsValid)
            {
                var response = await _borrowRequestService.CreateBorrowRequest(Request);
                if (!response.Success)
                    return LocalRedirect("");
                ModelState.AddModelError("", response.ValidationErrors);
                return Page();
            }

            return Page();
        }
    }
}
