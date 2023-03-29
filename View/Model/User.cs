namespace View.Model
{
    public class UserLogin
    {
        public string UserName { get; set;}
        public string Password { get; set;}

    }

    public class UserRegister:UserLogin
    {
        public string Email { get; set; }
    }
}
