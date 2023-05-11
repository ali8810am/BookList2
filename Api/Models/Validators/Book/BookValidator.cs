using FluentValidation;

namespace Api.Models.Validators.Book
{
    public class BookValidator:AbstractValidator<CreateBookDto>
    {
        public BookValidator()
        {
            RuleFor(b => b.Name)
                .Length(2, 200).WithMessage("{PropertyName} length must be between 2 and 200 charactors");
            RuleFor(b => b.Author)
                .Length(2, 50).WithMessage("{PropertyName} length must be between 2 and 50 charactors");

        }
    }
}
