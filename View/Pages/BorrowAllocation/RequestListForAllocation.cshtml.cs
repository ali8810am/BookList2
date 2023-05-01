using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;
using View.Model;

namespace View.Pages.BorrowAllocation
{
    public class RequestListForAllocationModel : PageModel
    {
        private readonly IBorrowRequestService _borrowRequestService;
        private readonly IBorrowAllocationService _borrowAllocationService;
        private readonly IUserService _userService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public RequestListForAllocationModel(IBorrowRequestService borrowRequestService, IBorrowAllocationService borrowAllocationService, IUserService userService, IEmployeeService employeeService, IMapper mapper)
        {
            _borrowRequestService = borrowRequestService;
            _borrowAllocationService = borrowAllocationService;
            _userService = userService;
            _employeeService = employeeService;
            _mapper = mapper;
        }
        [BindProperty]
        public List<BorrowRequestForAllocationListVm>? Requests { get; set; }
        public async Task OnGet()
        {
            var requests = await _borrowRequestService.GetBorrowRequests();
            Requests = _mapper.Map<List<BorrowRequestForAllocationListVm>>(requests);
        }

        public async Task<IActionResult> OnPost()
        {
            Requests = Requests.Where(r => r.Allocate == true).ToList();
            var allocationList = new List<CreateBorrowAllocationVm>();
            foreach (var req in Requests)
            {
                var request = await _borrowRequestService.GetBorrowRequest(req.RequestId, new List<string> { "Book" });
                var id = _userService.GetCurrentUserId();
                var emp =await _employeeService.GetEmployeeByUserId(id);
                var allocate = new CreateBorrowAllocationVm
                {
                    BookId = request.BookId,
                    BorrowEndDate = request.EndDate,
                    BorrowStartDate = request.StartDate,
                    CustomerId = request.CustomerId,
                    DateApproved = DateTime.Now,
                    IsReturned = false,
                    EmployeeId = emp.EmployeeId
                };
                allocationList.Add(allocate);
            }

            foreach (var all in allocationList)
            {
               await _borrowAllocationService.CreateBorrowAllocation(all);
            }
            Requests = Requests.Where(r => r.Allocate == true||r.Reject==true).ToList();

            foreach (var req in Requests)
            {
                await _borrowRequestService.DeleteBorrowRequest(req.RequestId);

            }

            return LocalRedirect("/");
        }
    }
}
