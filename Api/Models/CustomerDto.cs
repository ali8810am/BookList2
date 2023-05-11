using Api.Data;
using Api.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateCustomerDto
    {
        public string UserId { get; set; }
        
        [Required]
        public int MembershipRate { get; set; }
    }

    public class CustomerDto:CreateCustomerDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }

    }
}
