using Api.Models.Identity;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Api.Models.Validators.UserValidator
{
    public class RegisterValidator:AbstractValidator<RegisterRequestDto>
    {
        public RegisterValidator()
        {
            RuleFor(u => u.FirstName)
                .Length(2, 20).WithMessage("{PropertyName} length must be between 2 and 20 charactors");
            RuleFor(u => u.LastName)
                .Length(2, 20).WithMessage("{PropertyName} length must be between 2 and 20 charactors");
            RuleFor(u => u.UserName)
                .Length(2, 20).WithMessage("{PropertyName} length must be between 2 and 20 charactors");
            RuleFor(u => u.PhoneNumber)
                .Matches(@"^[0-9]{11}$").WithMessage("invalid {PropertyName}");

        }
    }
}
