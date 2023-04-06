using Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateBorrowAllocationDto
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BorrowStartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BorrowEndDate { get; set; }
        public BookDto Book { get; set; }
        public int BookId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateApproved { get; set; }
        public Employee Employee { get; set; }
        public string EmployeeId { get; set; }
        public Customer Customer { get; set; }
        public string CustomerId { get; set; }
        public bool IsReturned { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DateReturned { get; set; }
    }

    public class BorrowAllocationDto:CreateBorrowAllocationDto
    {
        public int AllocationId { get; set; }
    }
}
