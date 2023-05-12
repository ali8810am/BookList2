using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using View.ConstantParameters;
using View.Contracts;
using View.Model;
using View.Services.Base;

namespace View.Pages.User
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty] public UserRegisterVm? RegisterVm { get; set; }
        public string ReturnUrl { get; set; }
        [BindProperty] public string Role { get; set; } 

        public void OnGet(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPost()
        {
            var existUser = new ExistUserDto{Email = RegisterVm.Email,PhoneNumber = RegisterVm.PhoneNumber,UserName = RegisterVm.UserName};
            var isExistUser = await _userService.ExistUser(existUser);
            if (isExistUser.ExistEmail)
            {
                ModelState.AddModelError("", "User with this email is already exist");
                return Page();
            }
            if (isExistUser.ExistUsername)
            {
                ModelState.AddModelError("", "User with this username is already exist");
                return Page();
            }
            if (isExistUser.ExistPhoneNumber)
            {
                ModelState.AddModelError("", "User with this phone number is already exist");
                return Page();
            }

            RegisterVm.Roles = new List<string>();
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Register attemped is failed");
                return Page();
            }
            
            var returnUrl = Url.Content("~/");
            var response = await _userService.Register(RegisterVm);
            if (response.Success)
                return LocalRedirect(returnUrl);
            ModelState.AddModelError("",response.Errors);
            return Page();

        }
    }
}
