using Api.Data;
using Api.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateCustomerDto
    {
        public int UserId { get; set; }
   
        public DateTime DateMembered { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public int MembershipRate { get; set; }
    }

    public class CustomerDto:CreateCustomerDto
    {
        public int CustomerId { get; set; }
        public UserDto User { get; set; }

    }
}
