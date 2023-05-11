using Api.IRepository;
using FluentValidation;

namespace Api.Models.Validators.BorrowRequest
{
    public class UpdateBorrowRequestValidator:AbstractValidator<BorrowRequestDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IBookRepository _bookRepository;

        public UpdateBorrowRequestValidator(ICustomerRepository customerRepository, IBookRepository bookRepository)
        {
            _customerRepository = customerRepository;
            _bookRepository = bookRepository;
            Include(new CreateBorrowRequestValidator(_bookRepository,_customerRepository));
            RuleFor(r => r.RequestId)
                .GreaterThan(0)
                .NotNull().WithMessage("{PropertyName} must be greater than 0 and not null");

        }
    }
}
