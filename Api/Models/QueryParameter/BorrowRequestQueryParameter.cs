namespace Api.Models.QueryParameter
{
    public class BorrowRequestQueryParameter:QueryParameter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BookId { get; set; }
        public bool? Approved { get; set; }
        public bool? Cancelled { get; set; }
        public int? CustomerId { get; set; }
    }
}
