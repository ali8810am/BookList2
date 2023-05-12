using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using View.ConstantParameters;
using View.Contracts;
using View.Model;

namespace View.Pages.BorrowRequest
{
    [Authorize(Roles = $"{UserRoles.Customer},{UserRoles.Admin}")]
    public class CreateModel : PageModel
    {
        private readonly IBorrowRequestService _borrowRequestService;
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CreateModel(IBorrowRequestService borrowRequestService, IBookService bookService, IUserService userService, ICustomerService customerService, IMapper mapper)
        {
            _borrowRequestService = borrowRequestService;
            _bookService = bookService;
            _userService = userService;
            _customerService = customerService;
            _mapper = mapper;
        }
        [BindProperty]
        public CreateBorrowRequestVm Request { get; set; } = new CreateBorrowRequestVm();

        public CreateBookVm BookVm { get; set; }

        public async Task OnGet(int bookId)
        {
            var book = await _bookService.GetBook(bookId);
            BookVm = _mapper.Map<CreateBookVm>(book);
            Request.BookId = bookId;
            Request.StartDate=DateTime.Now;
            Request.EndDate = Request.StartDate.AddDays(1);
        }

        public async Task<IActionResult> OnPost()
        {
            Request.DateRequested = DateTime.Now;
            var userId = _userService.GetCurrentUserId();
            var customer = await _customerService.GetCustomerByUserId(userId);
            Request.CustomerId = customer.CustomerId;
            if (ModelState.IsValid)
            {
                var response = await _borrowRequestService.CreateBorrowRequest(Request);
                if (response.Success)
                    return LocalRedirect("/BorrowRequest/Index");
                ModelState.AddModelError("", response.ValidationErrors);
                return Page();
            }

            return Page();
        }
    }
}
