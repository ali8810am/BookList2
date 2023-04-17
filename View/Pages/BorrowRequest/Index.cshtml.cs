using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;
using View.Model;

namespace View.Pages.BorrowRequest
{
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
