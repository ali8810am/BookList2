using BookList2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace BookList2.Pages.Booklist
{
    public class CreateModel : PageModel
    {
        private ApplicationDbContext _context;
        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Book Book { get; set; }
        public void OnGet()
        {
         
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(Book);
                _context.SaveChanges();
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }

    }
}
