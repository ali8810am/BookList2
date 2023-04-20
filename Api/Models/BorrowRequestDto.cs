using System.ComponentModel.DataAnnotations;
using Api.Data;

namespace Api.Models
{
    public class CreateBorrowRequestDto
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateRequested { get; set; }
        [StringLength(250, MinimumLength = 2)]
        public string RequestComments { get; set; }
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }
        public string? CreatedBy { get; set; }

    }

    public class BorrowRequestDto:CreateBorrowRequestDto
    {
        public int RequestId { get; set; }
        public BookDto Book { get; set; }
        public CustomerDto Customer { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class UpdateBorrowRequestDto
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateRequested { get; set; }
        [StringLength(250, MinimumLength = 2)]
        public string RequestComments { get; set; }
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }
    }
}
