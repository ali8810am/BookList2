using System.ComponentModel.DataAnnotations;

namespace Api.Models.Identity
{
    public class LoginRequestDto
    {

        [Required]
        [StringLength(maximumLength: 15, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class LoginResponseDto:RegisterResponseDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }

    public class RegisterResponseDto
    {
        public string UserId { get; set; }
    }
    public class RegisterRequestDto:LoginRequestDto
    {
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public ICollection<string>? Roles { get; set; }
    }

    public class UserDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
