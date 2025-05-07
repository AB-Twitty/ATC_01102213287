using Evenda.UI.Contracts.IServices;
using System.Net;
using System.Net.Http.Headers;

namespace Evenda.UI.Handlers
{
    public class AuthTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiTokenService _apiTokenService;

        public AuthTokenHandler(IHttpContextAccessor accessor, IApiTokenService apiTokenService)
        {
            _httpContextAccessor = accessor;
            _apiTokenService = apiTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = _httpContextAccessor.HttpContext?.Session.GetString("access-token");

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshed = await _apiTokenService.TryRefreshToken();

                if (refreshed)
                {
                    var newAccessToken = _httpContextAccessor.HttpContext?.Session.GetString("access-token");

                    if (!string.IsNullOrEmpty(newAccessToken))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
                        response.Dispose();
                        return await base.SendAsync(request, cancellationToken);
                    }
                }
            }

            return response;
        }
    }
}
