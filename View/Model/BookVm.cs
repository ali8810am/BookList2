using System;
using System.ComponentModel.DataAnnotations;

namespace View.Model
{
    public class CreateBookVm
    {
       


        [Required(ErrorMessage ="Please enter books name")]
        [MaxLength(300)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Author { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }= DateTime.Now;
        public DateTime? DateBackToLibrary { get; set; }
        public bool IsInLibrary { get; set; } = true;

    }

    public class BookVm:CreateBookVm
    {
        public int Id { get; set; }
    }
}
