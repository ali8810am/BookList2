namespace Api.Exceptions
{
    public class NotFoundException:ApplicationException
    {
        public NotFoundException(string message,object key): base($"{message} and {key} was not found")
        {
            
        }
    }
}
