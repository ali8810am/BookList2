using Api.IRepository;
using FluentValidation;

namespace Api.Models.Validators.BorrowAllocation
{
    public class UpdateBorrowAllocationValidator:AbstractValidator<BorrowAllocationDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBookRepository _bookRepository;

        public UpdateBorrowAllocationValidator(ICustomerRepository customerRepository, IEmployeeRepository employeeRepository, IBookRepository bookRepository)
        {
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _bookRepository = bookRepository;
            Include(new CreateBorrowAllocationValidator(_customerRepository,_employeeRepository,_bookRepository));
            RuleFor(a => a.AllocationId)
                .GreaterThan(0).NotNull().WithMessage("{PropertyName} must be greater than 0 and not null");
        }
    }
}
