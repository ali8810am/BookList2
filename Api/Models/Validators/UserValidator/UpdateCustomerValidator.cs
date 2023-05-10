using Api.Services;
using FluentValidation;

namespace Api.Models.Validators.UserValidator
{
    public class UpdateCustomerValidator : AbstractValidator<CustomerDto>
    {
        private readonly IAuthManager _authManager;

        public UpdateCustomerValidator(IAuthManager authManager)
        {
            _authManager = authManager;
            Include(new CustomerValidator(_authManager));
            RuleFor(a => a.Id)
                .GreaterThan(0).NotNull().WithMessage("{PropertyName} must be greater than 0 and not null");
        }
    }
}
