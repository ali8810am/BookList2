using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using View.ConstantParameters;
using View.Contracts;
using View.Model;

namespace View.Pages.BorrowAllocation
{
    [Authorize(Roles = $"{UserRoles.Employee},{UserRoles.Admin}")]
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
        public CreateBorrowAllocationVm Allocation { get; set; }
        [BindProperty]
        public int Id { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            var all = await _borrowAllocationService.GetBorrowAllocation(id,
                new List<string> { "Book", "Employee", "Customer" });
            Allocation =  _mapper.Map<CreateBorrowAllocationVm>(all);
            if (Allocation == null)
                return NotFound();
            Id = all.AllocationId;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Allocation.CreateBy = "NotAuthenticated";
            if (ModelState.IsValid)
            {
                var response = await _borrowAllocationService.UpdateBorrowAllocation(Id,Allocation);
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
