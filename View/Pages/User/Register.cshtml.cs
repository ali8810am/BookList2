using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using View.ConstantParameters;
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
        [BindProperty] public string Role { get; set; } 

        public void OnGet(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPost()
        {
            if (Role==null)
            {
                Role = UserRoles.Admin;
            }
            RegisterVm.Roles.Add(Role);
            if (Role==UserRoles.Admin)
            {
                RegisterVm.Roles.Add(UserRoles.Customer);
                RegisterVm.Roles.Add(UserRoles.Employee);

            }
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
