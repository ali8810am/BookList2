using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using View.Contracts;

namespace View.Pages.User
{
    public class LogoutModel : PageModel
    {
        private readonly IUserService _userService;

        public LogoutModel(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> OnGet(string returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            await _userService.Logout();
            return LocalRedirect(returnUrl);
        }
    }
}
