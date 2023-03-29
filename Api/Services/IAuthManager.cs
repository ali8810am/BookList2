using Api.Models;

namespace Api.Services
{
    public interface IAuthManager
    {
        Task<string> CreateToken();
        Task<bool> ValidateUser(LoginDto user);
    }
}
