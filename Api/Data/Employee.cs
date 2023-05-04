using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data
{
    public class Employee : BaseDomainObject
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApiUser User { get; set; }=new ApiUser();
        public DateTime DateHired { get; set; }
        public string Duty { get; set; }
        public virtual List<BorrowAllocation>? BorrowAllocations { get; set; }
    }
}
