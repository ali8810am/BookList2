namespace Api.Models.QueryParameter
{
    public class BorrowRequestQueryParameter:QueryParameter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? BookId { get; set; }
        public DateTime? DateRequested { get; set; }
        public string? RequestComments { get; set; }
        public bool? Approved { get; set; }
        public bool? Cancelled { get; set; }
        public string? CreatedBy { get; set; }
    }
}
