using System.ComponentModel.DataAnnotations;

namespace BookList.Infrastructure.Data
{
    public class Book:BaseDomainObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter books name")]
        [MaxLength(300)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Author { get; set; }

    }
}
