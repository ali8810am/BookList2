﻿using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateBookDto
    {
        [Required]
        [StringLength(200,MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Author { get; set; }
    }

    public class BookDto:CreateBookDto
    {
        public int Id { get; set; }
    }
}
