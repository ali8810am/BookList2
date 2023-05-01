using View.Model;
using View.Services.Base;

namespace View.Contracts
{
    public interface IBorrowAllocationService
    {
        Task<List<BorrowAllocationVm>> GetBorrowAllocations();
        Task<BorrowAllocationVm> GetBorrowAllocation(int id, List<string>? include);
        Task<Response<int>> CreateBorrowAllocation(CreateBorrowAllocationVm borrowAllocation);
        Task<Response<int>> UpdateBorrowAllocation(BorrowAllocationVm borrowAllocation);
        Task<Response<int>> DeleteBorrowAllocation(int id);
    }
}
