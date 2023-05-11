using Api.Data;
using Api.IRepository;
using Api.Models.Identity;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private IBookRepository _books;
        private IBorrowAllocationRepository _borrowAllocations;
        private IBorrowRequestRepository _borrowRequests;
        private ICustomerRepository _customerRepository;
        private IEmployeeRepository _employees;

        public UnitOfWork(ApplicationDbContext context,IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor=contextAccessor;
            _context=context;
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IBookRepository Books => _books ??= new BookRepository(_context);
        public IBorrowAllocationRepository BorrowAllocations => _borrowAllocations ??= new BorrowAllocationRepository(_context);
        public IBorrowRequestRepository BorrowRequests => _borrowRequests ??= new BorrowRequestRepository(_context);
        public ICustomerRepository Customers => _customerRepository??=new CustomerRepository(_context);
        public IEmployeeRepository Employee =>_employees??=new EmployeeRepository(_context);

        public async Task Save()
        {
            var username = _contextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.UId)?.Value;
            await _context.SaveChangesAsync(username);
        }
    }
}
