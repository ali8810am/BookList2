using Api.Data;

namespace Api.Services
{
    public interface IUserService
    {
        string GetCurrentUserName();
        ApiUser GetUserByUserId(string userId);
    }
}
