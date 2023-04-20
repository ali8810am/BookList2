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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public EditModel(IBorrowRequestService borrowRequestService, IUserService userService, IMapper mapper)
        {
            _borrowRequestService = borrowRequestService;
            _userService = userService;
            _mapper = mapper;
        }
        [BindProperty]
        public BorrowRequestVm Request { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            Request = await _borrowRequestService.GetBorrowRequest(id,new List<string>{"Book"});
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
                    return LocalRedirect("/BorrowRequest/Index");
                }
                ModelState.AddModelError("",response.ValidationErrors);
                return Page();
            }
            return Page();
        }
    }
}
