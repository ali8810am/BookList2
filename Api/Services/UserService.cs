using Api.Data;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public class UserService:IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly UserManager<ApiUser> _userManager;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<ApiUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            //_userManager = userManager;
        }
        public string GetCurrentUserName()
        {
               var name= _httpContextAccessor.HttpContext.User.Identity.Name ?? "NotAuthenticated";

               return name;
        }
       
    }
}
