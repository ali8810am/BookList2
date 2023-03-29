using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using View.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Model;

namespace View.Pages.Booklist
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
          
        }

        public async Task<IActionResult> OnGetDelete(int ID)
        {
            var book = _context.Books;
            if (book == null)
            {
                return NotFound();
            }
            else
            {
              
                return RedirectToAction("Index");
            }
        }
    }
}
