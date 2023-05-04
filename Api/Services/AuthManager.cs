using System.Data;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly IConfiguration _config;
        private ApiUser _user;

        public AuthManager(UserManager<ApiUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApiUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
        }
        private async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var jwtSetting = _config.GetSection("JwtSettings");
            var token = new JwtSecurityToken(
                issuer: jwtSetting.GetSection("Issuer").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSetting.GetSection("expireTime").Value)),
                signingCredentials: signingCredentials
            );
            return token;
        }

        private async Task<IEnumerable<Claim>> GetClaims()
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
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("JWT_KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
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
                    throw new NotFoundException($"Credentials for '{request.UserName} aren't valid'.",request);
                }

                var jwtSecurityToken = await CreateToken();

                var response = new LoginResponseDto()
                {
                    UserId = _user.Id,
                    Token = jwtSecurityToken,
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

            var creationResult = await _userManager.CreateAsync(user, request.Password);

            if (creationResult.Succeeded)
            {
                var validRoles = new List<string>();
                foreach (var role in request.Roles)
                {
                    switch (role.ToLower())
                    {
                        case UserRoles.User:
                            validRoles.Add(UserRoles.User);
                            break;
                        case UserRoles.Admin:
                            validRoles.Add(UserRoles.Admin);
                            break;
                        case UserRoles.Customer:
                            validRoles.Add(UserRoles.Customer);
                            break;
                        case UserRoles.Employee:
                            validRoles.Add(UserRoles.Employee);
                            break;
                    }
                }
                foreach (var role in validRoles)
                {
                    var roleExist = await _roleManager.RoleExistsAsync(role);
                    if (roleExist == false)
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    var roleResult = await _userManager.AddToRoleAsync(user,role);
                    if (!roleResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(_user);
                        throw new BadRequestException("sorry. Please try again");
                    }
                }
                return new RegisterResponseDto { UserId = user.Id };
            }
            else
            {
                StringBuilder str = new StringBuilder();
                foreach (var err in creationResult.Errors)
                {
                    str.AppendFormat("•{0}\n", err.Description);
                }

                throw new BadRequestException($"{str}");
            }
        }
        public ApiUser GetUserByUserId(string userId)
        {
            var user = new ApiUser();
            user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
            return user;
        }
    }
}
