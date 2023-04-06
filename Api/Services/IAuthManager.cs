using System.IdentityModel.Tokens.Jwt;
using Api.Models;
using Api.Models.Identity;

namespace Api.Services
{
    public interface IAuthManager
    {
        Task<LoginResponseDto> Login(LoginRequestDto request);
        Task<RegisterResponseDto> Register(RegisterRequestDto request);
    }
}
