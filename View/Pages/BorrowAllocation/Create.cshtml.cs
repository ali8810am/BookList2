using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using View.Contracts;
using View.Model;
using View.Services;

namespace View.Pages.BorrowAllocation
{
    public class CreateModel : PageModel
    {
        private readonly IBorrowAllocationService _borrowAllocationService;
        private readonly IBorrowRequestService _borrowRequestService;
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateModel(IBorrowAllocationService borrowAllocationService, IBorrowRequestService borrowRequestService, IEmployeeService employeeService, IUserService userService, IMapper mapper)
        {
            _borrowAllocationService = borrowAllocationService;
            _borrowRequestService = borrowRequestService;
            _employeeService = employeeService;
            _userService = userService;
            _mapper = mapper;
        }
        [BindProperty]
        public CreateBorrowAllocationVm Allocation { get; set; }= new CreateBorrowAllocationVm();

        public string BookName { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            var request = await _borrowRequestService.GetBorrowRequest(id, new List<string> { "Book" });
            if (Request == null)
                return NotFound();
            BookName = request.Book.Name;
       

            Allocation.BookId = request.BookId;
            Allocation.BorrowEndDate = request.EndDate;
            Allocation.BorrowStartDate = request.StartDate;
            Allocation.CustomerId = request.CustomerId;
            Allocation.DateApproved = DateTime.Now;
            Allocation.IsReturned = false;
    
            
            
            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            Allocation.CreateBy = _userService.GetCurrentUserName();
            if (ModelState.IsValid)
            {
                var userId = _userService.GetCurrentUserId();
                var emp = await _employeeService.GetEmployeeByUserId(userId);
                Allocation.EmployeeId = emp.EmployeeId;
            await _borrowAllocationService.CreateBorrowAllocation(Allocation);
                //if (response.Success)
                //{
                //    return LocalRedirect("/BorrowRequest/Index");
                //}
                //ModelState.AddModelError("", response.ValidationErrors);
                return Page();
            }
            return Page();
        }
    }
}
