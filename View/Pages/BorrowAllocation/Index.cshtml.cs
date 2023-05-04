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

    public class IndexModel : PageModel
    {
        private readonly IBorrowAllocationService _borrowAllocationService;

        public IndexModel(IBorrowAllocationService borrowAllocationService)
        {
            _borrowAllocationService = borrowAllocationService;
        }

        public List<BorrowAllocationVm> Allocations { get; set; }
        public async Task OnGet()
        {
            Allocations = await _borrowAllocationService.GetBorrowAllocations();
        }
    }
}
