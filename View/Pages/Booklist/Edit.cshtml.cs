using View.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Model;

namespace View.Pages.Booklist
{
    public class EditModel : PageModel
    {
        private ApplicationDbContext _dbContext;
        public EditModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [BindProperty]
        public Book Book { get; set; }
        public void OnGet(int ID)
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
