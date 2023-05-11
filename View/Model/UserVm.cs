using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
         public List<string>? Roles { get; set; }
    }

    public class UserVm
    {
        public string UserName { get; set; }
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

    public class EmployeeVm:CreateEmployeeVm
    {
        public int EmployeeId { get; set; }
        public UserVm User { get; set; }

    }

    public class CreateEmployeeVm
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
  
        public DateTime DateHired { get; set; }
        public string Duty { get; set; }
    }

    public class CustomerVm:CreateCustomerVm
    {
        public int CustomerId { get; set; }
       
    }

    public class CreateCustomerVm
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public UserVm User { get; set; }
        public DateTime DateMembered { get; set; }
        public int MembershipRate { get; set; }
    }
}
