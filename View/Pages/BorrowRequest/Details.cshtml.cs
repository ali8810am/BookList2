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
    public class DetailsModel : PageModel
    {
        private readonly IBorrowRequestService _borrowRequestService;

        public DetailsModel(IBorrowRequestService borrowRequestService)
        {
            _borrowRequestService = borrowRequestService;
        }
        [BindProperty]
        public BorrowRequestVm Request { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            Request = await _borrowRequestService.GetBorrowRequest(id, new List<string> { "Book","Customer" });
            if (Request == null)
                return NotFound();
            return Page();
        }
        public async Task<IActionResult> OnGetDelete(int id)
        {
            await _borrowRequestService.DeleteBorrowRequest(id);
            return RedirectToPage("/BorrowRequest/Index");
        }
    }
}
