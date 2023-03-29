using View.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using View.Model;

namespace View.Pages.Booklist
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
                
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }

    }
}
