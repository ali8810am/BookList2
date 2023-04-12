namespace View.Services.Base
{
    public partial class Client : View.Services.Base.IClient
    {
        public HttpClient HttpClient
        {
            get
            {
                return _httpClient;
            }
        }
    }
}
