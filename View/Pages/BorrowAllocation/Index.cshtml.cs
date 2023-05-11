using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using AutoMapper;
using View.ConstantParameters;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Pages.BorrowAllocation
{
    [Authorize(Roles = $"{UserRoles.Employee},{UserRoles.Admin}")]

    public class IndexModel : PageModel
    {
        private readonly IBorrowAllocationService _borrowAllocationService;
        private readonly IMapper _mapper;

        public IndexModel(IBorrowAllocationService borrowAllocationService, IMapper mapper)
        {
            _borrowAllocationService = borrowAllocationService;
            _mapper = mapper;
        }
        [BindProperty]
        public List<BorrowAllocationVm> Allocations { get; set; }
        public async Task OnGet()
        {
            Allocations = await _borrowAllocationService.GetBorrowAllocations(false);
        }

        public async Task<IActionResult> OnPost()
        {
            Allocations = Allocations.Where(a => a.IsReturned == true).ToList();
            var updateAllocationVm = new List<UpdateBorrowAllocationVm>();
            var allocationsVm = new List<BorrowAllocationVm>();
            var allocationVm = new BorrowAllocationVm();
            foreach (var allocation in Allocations)
            {
                allocationVm = await _borrowAllocationService.GetBorrowAllocation(allocation.AllocationId, new List<string>{});
                allocationVm.IsReturned=true;
                allocationsVm.Add(allocationVm);
            }

            foreach (var allocation in allocationsVm)
            {

              var result=  await _borrowAllocationService.UpdateBorrowAllocation(allocation.AllocationId, _mapper.Map<CreateBorrowAllocationVm>(allocation));
            }

            return LocalRedirect("/BorrowAllocation/Index");
        }
    }
}
