using System.ComponentModel.DataAnnotations;

namespace Api.Data
{
    public class Book:BaseDomainObject
    {
        public string Name { get; set; }
        public string Author { get; set; }

        public DateTime? DateBackToLibrary { get; set; }
        public bool IsInLibrary { get; set; } = true;
        public virtual List<BorrowAllocation> BorrowAllocations { get; set; }
        public virtual List<BorrowRequest> BorrowRequests { get; set; }


    }
}
