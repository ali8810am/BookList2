namespace View.Services.Base
{
    public class Response<T>
    {
        public string? Message { get; set; }
        public string? ValidationErrors { get; set; }
        public bool Success { get; set; }=false;
        public T? Data { get; set; }
    }
}
