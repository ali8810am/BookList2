namespace Api.Models.QueryParameter
{
    public class QueryParameter
    {
        public RequestParameters? RequestParameters { get; set; }=new RequestParameters();
        public List<string>? includes { get; set; }
    }
}
