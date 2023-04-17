using Api.Data;
using Api.IRepository;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IBookRepository _books;
        private IBorrowAllocationRepository _borrowAllocations;
        private IBorrowRequestRepository _borrowRequests;
        private ICustomerRepository _customerRepository;
        private IEmployeeRepository _employees;

        public UnitOfWork(ApplicationDbContext context, IBookRepository books, IBorrowAllocationRepository borrowAllocations, IBorrowRequestRepository borrowRequests, ICustomerRepository customerRepository, IEmployeeRepository employees)
        {
            _context = context;
            _books = books;
            _borrowAllocations = borrowAllocations;
            _borrowRequests = borrowRequests;
            _customerRepository = customerRepository;
            _employees = employees;
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
           await _context.SaveChangesAsync();
        }
    }
}
