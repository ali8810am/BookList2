using System.Diagnostics.CodeAnalysis;
using Api.Services;
using FluentValidation;

namespace Api.Models.Validators.UserValidator
{
    public class EmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        private readonly IAuthManager _authManager;

        public EmployeeValidator(IAuthManager authManager)
        {
            _authManager = authManager;
            RuleFor(e=>e.Duty)
                .Length(2, 50).WithMessage("{PropertyName} length must be between 2 and 50 charactors");
            RuleFor(r => r.UserId)
                .MustAsync(async (id, token) =>
                {
                    var bookExists = await _authManager.ExistUser(id);
                    return bookExists;
                });
        }
    }
}
