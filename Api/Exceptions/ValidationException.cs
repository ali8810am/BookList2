using System.Collections;
using FluentValidation.Results;

namespace Api.Exceptions
{
    public class ValidationException:ApplicationException, IEnumerable
    {
        public List<string> Errors { get; set; }=new List<string>();

        public ValidationException(ValidationResult validationException)
        {
            foreach (var error in validationException.Errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
