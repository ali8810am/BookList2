using Api.Services;
using FluentValidation;

namespace Api.Models.Validators.UserValidator
{
    public class CustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        private readonly IAuthManager _authManager;

        public CustomerValidator(IAuthManager authManager)
        {
            _authManager = authManager;

            _authManager = authManager;
            RuleFor(e => e.MembershipRate)
                .GreaterThan(0)
                .WithMessage("{PropertyName} length must greater than 0")
                .LessThan(6).WithMessage("{PropertyName} length must less than 6");
            RuleFor(r => r.UserId)
                .MustAsync(async (id, token) =>
                {
                    var bookExists = await _authManager.ExistUser(id);
                    return bookExists;
                });
        }
    }
}
