using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using View.Contracts;
using View.Model;

namespace View.Pages.User
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public UserRegisterVm? RegisterVm { get; set; }
        public string ReturnUrl { get; set; }
        public void OnGet(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var returnUrl = Url.Content("~/");
                var isCreated = await _userService.Register(RegisterVm);
                if (isCreated)
                    return LocalRedirect(returnUrl);
            }
            ModelState.AddModelError("", "Login attemped is failed");
            return Page();

        }
    }
}
