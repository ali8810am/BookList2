using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using View.ConstantParameters;
using View.Contracts;
using View.Model;
using View.Services;

namespace View.Pages.BorrowRequest
{
    [Authorize(Roles = $"{UserRoles.Customer},{UserRoles.Admin}")]
    public class IndexModel : PageModel
    {

        private readonly IBorrowRequestService _borrowRequestService;
        public IndexModel(IBorrowRequestService borrowRequestService)
        {
            _borrowRequestService = borrowRequestService;
        }
        public IEnumerable<BorrowRequestVm>? Requests { get; set; }
        public async Task OnGet()
        {
            Requests =await _borrowRequestService.GetBorrowRequests();
        }
       
    }
}
