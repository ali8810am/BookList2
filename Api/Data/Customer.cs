using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace Api.Data
{
    public class Customer:BaseDomainObject
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApiUser User { get; set; }= new ApiUser();
        public DateTime DateMembered { get; set; }
        public int MembershipRate { get; set; }
        public List<BorrowAllocation>? BorrowAllocations { get; set; }
        public List<BorrowRequest>? BorrowRequests { get; set; }
    }
}
