using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool Approved { get; set; }=false;
        public bool Cancelled { get; set; } = false;
        public int CustomerId { get; set; }
     
    }

    public class BorrowRequestVm : CreateBorrowRequestVm
    {
        public int RequestId { get; set; }
        [ForeignKey("CustomerId")]
        public CustomerVm? Customer { get; set; }
        public BookVm? Book { get; set; }
    }


    public class BorrowRequestForAllocationListVm : BorrowRequestVm
    {
        public bool Reject { get; set; } = false;
        public bool Allocate { get; set; } = false;
    }
}
