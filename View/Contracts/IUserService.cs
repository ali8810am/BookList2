using View.Model;
using View.Services.Base;

namespace View.Contracts
{
    public interface IUserService
    {
        Task<bool> Login(string userName, string password);
        Task Logout();
        Task<RegisterResponseVm> Register(UserRegisterVm user);
        string GetCurrentUserName();
        string GetCurrentUserId();
        Task<ExistUserResponse> ExistUser(ExistUserDto user);
    }
}
