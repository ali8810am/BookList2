using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;
using View.Model;

namespace View.Pages.BorrowAllocation
{
    public class EditModel : PageModel
    {
        private readonly IBorrowAllocationService _borrowAllocationService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public EditModel(IBorrowAllocationService borrowAllocationService, IUserService userService, IMapper mapper)
        {
            _borrowAllocationService = borrowAllocationService;
            _userService = userService;
            _mapper = mapper;
        }
        [BindProperty]
        public BorrowAllocationVm Allocation { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            Allocation = await _borrowAllocationService.GetBorrowAllocation(id,new List<string>{"Book","Customer","Employee"});
            if (Request == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            Allocation.UpdatedBy = _userService.GetCurrentUserName();
            if (ModelState.IsValid)
            {
                var response = await _borrowAllocationService.UpdateBorrowAllocation(Allocation);
                if (response.Success)
                {
                    return LocalRedirect("/BorrowRequest/Index");
                }
                ModelState.AddModelError("", response.ValidationErrors);
                return Page();
            }
            return Page();
        }
    }
}
