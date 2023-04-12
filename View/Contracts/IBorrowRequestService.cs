using View.Model;
using View.Services.Base;

namespace View.Contracts
{
    public interface IBorrowRequestService
    {
        Task<List<BorrowRequestVm>> GetBorrowRequests();
        Task<BorrowRequestVm> GetBorrowRequest(int id);
        Task<Response<int>> CreateBorrowRequest(CreateBorrowRequestVm borrowRequest);
        Task<Response<int>> UpdateBorrowRequest(BorrowRequestVm borrowRequest);
        Task<Response<int>> DeleteBorrowRequest(int id);
    }
}
