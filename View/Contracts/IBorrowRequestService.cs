using View.Model;
using View.Services.Base;

namespace View.Contracts
{
    public interface IBorrowRequestService
    {
        Task<List<BorrowRequestVm>> GetBorrowRequests();
        Task<List<BorrowRequestVm>> GetBorrowRequests(bool? approved);
        Task<List<BorrowRequestVm>> GetBorrowRequests(int id);
        Task<BorrowRequestVm> GetBorrowRequest(int id);
        Task<BorrowRequestVm> GetBorrowRequest(int id, List<string>? include);
        Task<Response<int>> CreateBorrowRequest(CreateBorrowRequestVm borrowRequest);
        Task<Response<int>> UpdateBorrowRequest(BorrowRequestVm borrowRequest);
        Task DeleteBorrowRequest(int id);
    }
}
