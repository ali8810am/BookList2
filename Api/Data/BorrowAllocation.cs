﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Data
{
    public class BorrowAllocation:BaseDomainObject
    {
        public DateTime BorrowStartDate { get; set; }
        public DateTime BorrowEndDate { get; set; }
        public DateTime DateApproved { get; set; }
        public bool IsReturned { get; set; }
        public DateTime? DateReturned { get; set; }

        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")] 
        public virtual Customer Customer { get; set; }
        public int RequestId { get; set; }

    }
}
