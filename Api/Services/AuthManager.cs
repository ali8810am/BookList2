using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.ConstantParameters;
using Api.Data;
using Api.Exceptions;
using Api.Models.Identity;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly JwtSetting _jwtSettings;
        private ApiUser? _user;

        public AuthManager(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager, JwtSetting jwtSettings, ApiUser user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _user = user;
        }

        public async Task<JwtSecurityToken> CreateToken()
        {
            var userClaims = await _userManager.GetClaimsAsync(_user);
            var roles = await _userManager.GetRolesAsync(_user);

            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                    new Claim(CustomClaimTypes.UId, _user.Id)
                }
                .Union(userClaims)
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public async Task<bool> ValidateUser(LoginRequestDto user)
        {
            _user = await _userManager.FindByNameAsync(user.UserName);
            if (_user == null)
            {
                throw new NotFoundException($"User {user.UserName} not found.", user.UserName);
            }
            return await _userManager.CheckPasswordAsync(_user, user.Password);
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            try
            {
                var validation = await ValidateUser(request);
                if (validation == false)
                {
                    throw new BadRequestException($"Credentials for '{request.UserName} aren't valid'.");
                }

                JwtSecurityToken jwtSecurityToken = await CreateToken();

                var response = new LoginResponseDto()
                {
                    UserId = _user.Id,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Email = _user.Email,
                    UserName = _user.UserName
                };
                return response;
            }
            catch (BadRequestException ex)
            {
                throw;
            }
        }

        public async Task<RegisterResponseDto> Register(RegisterRequestDto request)
        {
            var user = new ApiUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                return new RegisterResponseDto { UserId = user.Id };
            }
            else
            {
                StringBuilder str = new StringBuilder();
                foreach (var err in result.Errors)
                {
                    str.AppendFormat("•{0}\n", err.Description);
                }

                throw new BadRequestException($"{str}");
            }
        }
    }
}
