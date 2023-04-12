using System.ComponentModel.DataAnnotations;

namespace View.Model
{
    public class UserLoginVm
    {
        public string UserName { get; set;}
        public string Password { get; set;}

    }

    public class UserRegisterVm:UserLoginVm
    {
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
    public class LoginResponseVm
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
