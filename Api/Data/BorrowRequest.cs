using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data
{
    public class BorrowRequest: BaseDomainObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateRequested { get; set; }
        public string RequestComments { get; set; }
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }

        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
