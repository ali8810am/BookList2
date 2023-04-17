
namespace Api.Models.QueryParameter
{
    public class BorrowAllocationQueryParameter:QueryParameter
    {
        public DateTime? BorrowStartDate { get; set; }
 
        public DateTime? BorrowEndDate { get; set; }

        public int? BookId { get; set; }
        public DateTime? DateApproved { get; set; }

        public string EmployeeId { get; set; }=string.Empty;

        public string? CustomerId { get; set; } = string.Empty;
        public bool IsReturned { get; set; }

        public DateTime? DateReturned { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public string? UpdatedBy { get; set; } = string.Empty;
    }
}
