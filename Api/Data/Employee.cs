using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data
{
    public class Employee : BaseDomainObject
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApiUser User { get; set; }
        public DateTime DateHired { get; set; }
        public string Duty { get; set; }
        public List<BorrowAllocation> BorrowAllocations { get; set; }
    }
}
