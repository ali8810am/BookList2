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
    public class DetailsModel : PageModel
    {
        private readonly IBorrowAllocationService _borrowAllocationService;

        public DetailsModel(IBorrowAllocationService borrowAllocationService)
        {
            _borrowAllocationService = borrowAllocationService;
        }
        public BorrowAllocationVm Allocation { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            Allocation = await _borrowAllocationService.GetBorrowAllocation(id, new List<string> { "Book", "Employee", "Customer" });
            if (Request == null)
                return NotFound();
            return Page();
        }
        public async Task<IActionResult> OnGetDelete(int id)
        {
            await _borrowAllocationService.DeleteBorrowAllocation(id);
            return RedirectToPage("/BorrowRequest/Index");
        }
    }
}
