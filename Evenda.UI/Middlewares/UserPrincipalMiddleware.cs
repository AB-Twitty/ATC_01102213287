using Evenda.UI.Extensions;
using System.Security.Claims;

namespace Evenda.UI.Middlewares
{
    public class UserPrincipalMiddleware
    {
        private readonly RequestDelegate _next;

        public UserPrincipalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authSession = context.Session.GetUserSession();

            if (!string.IsNullOrEmpty(authSession.AccessToken) && !string.IsNullOrEmpty(authSession.Id.ToString()))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, authSession.Id.ToString()),
                    new Claim(ClaimTypes.Name, authSession.FirstName),
                    new Claim(ClaimTypes.Email, authSession.Email)
                };

                claims.AddRange(authSession.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var identity = new ClaimsIdentity(claims, "Session");
                var principal = new ClaimsPrincipal(identity);

                context.User = principal;
            }

            await _next(context);
        }
    }
}
