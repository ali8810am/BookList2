namespace Api.Responses
{
    public class ExistUserResponse
    {
        public bool ExistUsername { get; set; }=false;
        public bool ExistEmail { get; set; } = false;
        public bool ExistPhoneNumber { get; set; } = false;


    }
}
