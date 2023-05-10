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
    [Authorize(Roles =UserRoles.Customer)]
    
    public class IndexModel : PageModel
    {

        private readonly IBorrowRequestService _borrowRequestService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;

        public IndexModel(IBorrowRequestService borrowRequestService, IUserService userService, ICustomerService customerService)
        {
            _borrowRequestService = borrowRequestService;
            _userService = userService;
            _customerService = customerService;
        }
        public IEnumerable<BorrowRequestVm>? Requests { get; set; }
        public async Task OnGet()
        {
            var customer =await _customerService.GetCustomerByUserId(_userService.GetCurrentUserId());
            Requests =await _borrowRequestService.GetBorrowRequests(customer.CustomerId);
        }
       
    }
}
