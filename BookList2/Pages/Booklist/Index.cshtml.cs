using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookList2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookList2.Pages.Booklist
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext contexts)
        {
            _context = contexts;
        }
        public IEnumerable<Book> Books { get; set; }
        public string CreateDate { get; set; }
        public string PageName { get; set; }
        public void OnGet()
        {
            PageName = "Book List";
            Books = _context.Books.ToList();
        }
    }
}
