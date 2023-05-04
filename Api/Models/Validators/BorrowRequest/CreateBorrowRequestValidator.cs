using Api.IRepository;
using FluentValidation;

namespace Api.Models.Validators.BorrowRequest
{
    public class CreateBorrowRequestValidator:AbstractValidator<CreateBorrowRequestDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICustomerRepository _customerRepository;

        public CreateBorrowRequestValidator(IBookRepository bookRepository, ICustomerRepository customerRepository)
        {
            _bookRepository = bookRepository;
            _customerRepository = customerRepository;
            RuleFor(r => r.EndDate)
                .GreaterThan(r => r.StartDate).WithMessage("{PropertyName} must be after {ComparisonValue}");

            RuleFor(r => r.StartDate)
                .LessThan(r => r.EndDate).WithMessage("{PropertyName} must be before {ComparisonValue}");

            RuleFor(r => r.BookId)
                .GreaterThan(0)
                .MustAsync(async (id, token) => {
                    var bookExists = await _bookRepository.Exist(b=>b.Id==id);
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
        }
    }
}
