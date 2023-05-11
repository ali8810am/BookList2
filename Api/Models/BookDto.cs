using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class CreateBookDto
    {
    
        public string Name { get; set; }
     
        public string Author { get; set; }
        public DateTime? DateBackToLibrary { get; set; }
        public bool IsInLibrary { get; set; } = true;
    }

    public class BookDto:CreateBookDto
    {
        public int Id { get; set; }
    }
}
