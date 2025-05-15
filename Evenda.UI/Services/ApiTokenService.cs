using Evenda.UI.Contracts.IServices;
using Evenda.UI.Dtos.Auth;
using Evenda.UI.Helpers;
using Evenda.UI.Models.Response;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Evenda.UI.Services
{
    public class ApiTokenService : IApiTokenService
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        #endregion

        #region Ctor

        public ApiTokenService(IHttpContextAccessor accessor, HttpClient httpClient)
        {
            _httpContextAccessor = accessor;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ApiEndPoints.URI);
        }

        #endregion

        #region Methods

        public async Task<(bool, string)> TryRefreshToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            var refreshToken = user?.Claims.FirstOrDefault(c => c.Type == Constants.REFRESH_TOKEN_KEY)?.Value;
            var oldAccessToken = user?.Claims.FirstOrDefault(c => c.Type == Constants.ACCESS_TOKEN_KEY)?.Value;

            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(oldAccessToken)) return (false, "");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oldAccessToken);
            var response = await _httpClient.PostAsJsonAsync(ApiEndPoints.REFRESH_TOKEN, new RefreshTokenDto { RefreshToken = refreshToken });

            if (!response.IsSuccessStatusCode) return (false, "");

            var result = await response.Content.ReadFromJsonAsync<DataResponse<AuthDto>>();
            var auth = result?.Data ?? throw new Exception("Failed to refresh token");

            var claims = user?.Claims.Where(c =>
                c.Type != Constants.ACCESS_TOKEN_KEY && c.Type != Constants.REFRESH_TOKEN_KEY
            ).ToList() ?? [];

            claims.Add(new Claim(Constants.ACCESS_TOKEN_KEY, auth.AccessToken));
            claims.Add(new Claim(Constants.REFRESH_TOKEN_KEY, auth.RefreshToken));

            var identity = new ClaimsIdentity(claims, Constants.DEFAULT_AUTHENTICATION_SCHEME);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(Constants.DEFAULT_AUTHENTICATION_SCHEME, principal);

            return (true, auth.AccessToken);
        }

        #endregion
    }
}
