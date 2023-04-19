using View.Model;

namespace View.Contracts
{
    public interface IUserService
    {
        Task<bool> Login(string userName, string password);
        Task Logout();
        Task<bool> Register(UserRegisterVm user);
        string GetCurrentUserName();
        string GetCurrentUserId();
    }
}
