namespace Api.Services
{
    public class UserService:IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUserName()
        {
               var name= _httpContextAccessor.HttpContext.User.Identity.Name ?? "NotAuthenticated";

               return name;
        }
    }
}
