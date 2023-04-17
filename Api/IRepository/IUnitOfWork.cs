using Api.Data;

namespace Api.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        IBorrowAllocationRepository BorrowAllocations { get; }
        IBorrowRequestRepository BorrowRequests { get; }
        ICustomerRepository Customers { get; }
        IEmployeeRepository Employee { get; }
        Task Save();

    }
}
