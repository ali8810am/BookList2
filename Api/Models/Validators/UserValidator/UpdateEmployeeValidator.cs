using Api.Services;
using FluentValidation;

namespace Api.Models.Validators.UserValidator
{
    public class UpdateEmployeeValidator : AbstractValidator<EmployeeDto>
    {
        private readonly IAuthManager _authManager;

        public UpdateEmployeeValidator(IAuthManager authManager)
        {
            _authManager = authManager;
            Include(new EmployeeValidator(_authManager));
            RuleFor(a => a.Id)
                .GreaterThan(0).NotNull().WithMessage("{PropertyName} must be greater than 0 and not null");
        }
    }
}
