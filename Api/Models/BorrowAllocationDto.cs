﻿using Api.Data;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateBorrowAllocationDto
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
  
        public int EmployeeId { get; set; }

        public int CustomerId { get; set; }
        public bool IsReturned { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DateReturned { get; set; }
        public string? CreatedBy { get; set; }
       
    }

    public class BorrowAllocationDto:CreateBorrowAllocationDto
    {
        public int AllocationId { get; set; }
        public BookDto? Book { get; set; } = new BookDto();
        public EmployeeDto? Employee { get; set; }=new EmployeeDto();
        public CustomerDto? Customer { get; set; } = new CustomerDto();
        public string? UpdatedBy { get; set; }
    }
}
