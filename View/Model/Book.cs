using System;
using System.ComponentModel.DataAnnotations;

namespace View.Model
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Please enter books name")]
        [MaxLength(300)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Author { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }= DateTime.Now;
        //[Key]
        //public int ID { get; set; }
        //[Required]
        //[MaxLength(200)]
        //public string Name { get; set; }
        //[Required]
        //[MaxLength(200)]
        //public string Auther { get; set; }
        //public DateTime CreateDate { get; set; }

    }
}
