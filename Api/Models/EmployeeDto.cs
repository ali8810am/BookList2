using Api.Data;
using Api.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateEmployeeDto
    {
        public string UserId { get; set; }
     
        public DateTime DateHired { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Duty { get; set; }
    }

    public class EmployeeDto:CreateEmployeeDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
    }
}
