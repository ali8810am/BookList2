using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using View.Contracts;
using View.Services.Base;

namespace View.Middleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILocalStorageService _storage;

        public RequestMiddleware(RequestDelegate next, ILocalStorageService storage)
        {
            _next = next;
            _storage = storage;
        }

        private async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var ep = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
                var attribute = ep?.Metadata.GetMetadata<AuthorizeAttribute>();
                if (attribute != null)
                {
                    var tokenExist = _storage.Exists("token");
                    var tokenIsValid = true;
                    if (tokenExist)
                    {
                        var token = _storage.GetStorageValue<string>("token");
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenContent = tokenHandler.ReadJwtToken(token);
                        var expiry = tokenContent.ValidTo;
                        if (expiry < DateTime.Now)
                        {
                            tokenIsValid = false;
                        }
                    }

                    if (!tokenIsValid || !tokenExist)
                    {

                        return;
                    }

                    if (attribute.Roles != null)
                    {
                        var userRole = httpContext.User.FindFirst(ClaimTypes.Role).Value;
                        if (!attribute.Roles.Contains(userRole))
                        {
                            var path = $"/home/notauthorized";
                            httpContext.Response.Redirect(path);
                            return;
                        }
                    }
                }
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
                throw;
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            switch (e)
            {
                case ApiException:
                    await SignOutAndRedirect(httpContext);
                    break;
                default:
                    const string path = $"/home/error";
                    httpContext.Response.Redirect(path);
                    break;
            }
        }

        private static async Task SignOutAndRedirect(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            const string path = $"/user/login";
            httpContext.Response.Redirect(path);
        }
    }
}
