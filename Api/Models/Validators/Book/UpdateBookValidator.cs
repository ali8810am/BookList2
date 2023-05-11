using FluentValidation;

namespace Api.Models.Validators.Book
{
    public class UpdateBookValidator:AbstractValidator<BookDto>
    {
        public UpdateBookValidator()
        {
            Include(new BookValidator());
            RuleFor(b => b.Id)
                .GreaterThan(0).NotNull().WithMessage("{PropertyName} must be greater than 0 and not null");
        }
    }
}
