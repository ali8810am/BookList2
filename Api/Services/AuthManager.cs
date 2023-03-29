using Api.Models;

namespace Api.Services
{
    public class AuthManager:IAuthManager
    {
        public Task<string> CreateToken()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateUser(LoginDto user)
        {
            throw new NotImplementedException();
        }
    }
}
