using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using View.Contracts;
using View.Model;
using View.Services.Base;
using System.Security.Claims;
using Claim = System.Security.Claims.Claim;
using ClaimsIdentity = System.Security.Claims.ClaimsIdentity;
using ClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;
using Microsoft.Win32;

namespace View.Services
{
    public class UserService: BaseHttpService,IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IMapper _mapper;

        public UserService(ILocalStorageService localStorageService, IClient client, IHttpContextAccessor contextAccessor, IMapper mapper) : base(localStorageService, client)
        {
            this._contextAccessor = contextAccessor;
            this._jwtSecurityTokenHandler =new JwtSecurityTokenHandler();
            _mapper = mapper;
        }
        public async Task<bool> Login(string userName, string password)
        {
            try
            {
                _localStorageService.ClearStorage(new List<string> { "token" });
                LoginRequestDto authRequest = new LoginRequestDto { UserName = userName, Password = password };
                var authResponse = await _client.LoginAsync(authRequest);
                if (authResponse.Token != null)
                {
                    var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(authResponse.Token);
                    var claims = ParseClaims(tokenContent);
                    var user = new ClaimsPrincipal(new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme));
                    var login =_contextAccessor.HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, user);
                    _localStorageService.SetStorageValue(authResponse.Token, "token");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (ApiException e)
            {
                return false;
            }
        }

        

        public async Task Logout()
        {
            _localStorageService.ClearStorage(new List<string> { "token" });
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> Register(UserRegisterVm user)
        {
            var registrationRequest = _mapper.Map<RegisterRequestDto>(user);
            var response = await _client.RegisterAsync(registrationRequest);
            if (!string.IsNullOrEmpty(response.UserId))
            {
                return true;
            }
            return false;
        }
        public IList<Claim> ParseClaims(JwtSecurityToken tokenContent)
        {
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
            return claims;
        }
        public string GetCurrentUserName()
        {
            var name = _contextAccessor.HttpContext.User.Identity.Name ?? "NotAuthenticated";

            return name;
        }
        public string GetCurrentUserId()
        {
            var id = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c=>c.Type== "uid").Value;

            return id;
        }
    }
}
