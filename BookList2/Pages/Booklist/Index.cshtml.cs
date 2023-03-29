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

        public async Task<IActionResult> OnGetDelete(int ID)
        {
            var book = _context.Books.Find(ID);
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
    }
}
