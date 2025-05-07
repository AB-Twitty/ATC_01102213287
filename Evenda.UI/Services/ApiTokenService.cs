using Evenda.UI.Contracts.IServices;
using Evenda.UI.Dtos.Auth;
using Evenda.UI.Helpers;
using Evenda.UI.Models.Response;

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

        public async Task<bool> TryRefreshToken()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var refreshToken = session?.GetString("refresh-token");

            if (string.IsNullOrEmpty(refreshToken)) return false;

            var response = await _httpClient.PostAsJsonAsync(ApiEndPoints.REFRESH_TOKEN, new RefreshTokenDto { RefreshToken = refreshToken });

            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<DataResponse<AuthDto>>();
            var auth = result?.Data ?? throw new Exception("Failed to refresh token");

            session?.SetString("access-token", auth.AccessToken);
            session?.SetString("refresh-token", auth.RefreshToken);

            return true;
        }

        public void SetTokens(string accessToken, string refreshToken)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString("access-token", accessToken);
            session?.SetString("refresh-token", refreshToken);
        }

        #endregion
    }
}
