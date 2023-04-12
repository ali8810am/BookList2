﻿using System.ComponentModel.DataAnnotations;

namespace View.Model
{
    public class CreateBorrowAllocationVm
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BorrowStartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BorrowEndDate { get; set; }

        public int BookId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateApproved { get; set; }

        public string EmployeeId { get; set; }

        public string CustomerId { get; set; }
        public bool IsReturned { get; set; }
        [DataType(DataType.DateTime)] public DateTime? DateReturned { get; set; }
        public string? CreateBy { get; set; }
    }

    public class BorrowAllocationVm : CreateBorrowAllocationVm
    {
        public int AllocationId { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
