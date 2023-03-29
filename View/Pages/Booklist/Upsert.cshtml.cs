using System.Threading.Tasks;
using View.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using View.Model;

namespace View.Pages.Booklist
{
    public class UpsertModel : PageModel
    {
        private ApplicationDbContext _context;
        public UpsertModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Book Book { get; set; }
        public async Task<IActionResult> OnGet(int? id)
        {
            Book = new Book();
            if (id ==null)
            {
                return Page();
            }

           
            return Page();

        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
             
                }
                else
                {
                
                }
             
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
