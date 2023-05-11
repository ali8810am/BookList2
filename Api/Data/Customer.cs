using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace Api.Data
{
    public class Customer:BaseDomainObject
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApiUser User { get; set; }= new ApiUser();
        public DateTime DateMembered { get; set; }
        public int MembershipRate { get; set; }
        public virtual List<BorrowAllocation> BorrowAllocations { get; set; }
        public virtual List<BorrowRequest> BorrowRequests { get; set; }
    }
}
