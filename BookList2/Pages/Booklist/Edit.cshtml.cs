using BookList2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookList2.Pages.Booklist
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
            Book=_dbContext.Books.Find(ID);
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _dbContext.Books.Update(Book);
                _dbContext.SaveChanges();
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
