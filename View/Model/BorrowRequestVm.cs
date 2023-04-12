using System.ComponentModel.DataAnnotations;

namespace View.Model
{
    public class CreateBorrowRequestVm
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        public int BookId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateRequested { get; set; }

        [StringLength(250, MinimumLength = 2)] public string RequestComments { get; set; }
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class BorrowRequestVm : CreateBorrowRequestVm
    {
        public int RequestId { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
