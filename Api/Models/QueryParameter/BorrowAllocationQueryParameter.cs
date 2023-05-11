
namespace Api.Models.QueryParameter
{
    public class BorrowAllocationQueryParameter:QueryParameter
    {
        public DateTime? BorrowStartDate { get; set; } = new DateTime(2000, 1, 1);
 
        public DateTime? BorrowEndDate { get; set; } = new DateTime(2100, 1, 1);

        public int? BookId { get; set; }
        public DateTime? DateApproved { get; set; } = new DateTime(2000, 1, 1);

        public int? EmployeeId { get; set; }

        public int? CustomerId { get; set; } 
        public bool? IsReturned { get; set; }

        public DateTime? DateReturned { get; set; } = new DateTime(2000, 1, 1); 
        public string? CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; } = string.Empty;
    }
}
