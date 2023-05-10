using Api.IRepository;
using FluentValidation;

namespace Api.Models.Validators.BorrowAllocation
{
    public class CreateBorrowAllocationValidator:AbstractValidator<CreateBorrowAllocationDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBookRepository _bookRepository;

        public CreateBorrowAllocationValidator(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IBookRepository bookRepository)
        {
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _bookRepository = bookRepository;
            RuleFor(r => r.BorrowEndDate)
                .GreaterThan(r => r.BorrowStartDate).WithMessage("{PropertyName} must be after {ComparisonValue}");

            RuleFor(r => r.BorrowStartDate)
                .LessThan(r => r.BorrowEndDate).WithMessage("{PropertyName} must be before {ComparisonValue}");

            RuleFor(r => r.BookId)
                .GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var bookExists = await _bookRepository.Exist(b => b.Id == id&&b.IsInLibrary==true);
                    return bookExists;
                })
                .WithMessage("{PropertyName} does not exist.");

            RuleFor(p => p.CustomerId)
                .GreaterThan(0)
                .MustAsync(async (id, token) => {
                    var customerExists = await _customerRepository.Exist(b => b.Id == id);
                    return customerExists;
                })
                .WithMessage("{PropertyName} does not exist.");
            RuleFor(p => p.EmployeeId)
                .GreaterThan(0)
                .MustAsync(async (id, token) => {
                    var employeeExists = await _employeeRepository.Exist(b => b.Id == id);
                    return employeeExists;
                })
                .WithMessage("{PropertyName} does not exist.");
        }
    }
}
