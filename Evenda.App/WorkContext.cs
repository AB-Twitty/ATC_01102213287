using Evenda.App.Contracts;
using Evenda.App.Contracts.IInfrastructure.ITokenProvider;
using Microsoft.AspNetCore.Http;

namespace Evenda.App
{
    public class WorkContext : IWorkContext
    {
        #region Fields
        private string _accessToken;
        private string _userId;

        private readonly Microsoft.AspNetCore.Http.HttpContext _httpContext;
        private readonly ITokenProvider _tokenProvider;

        #endregion

        #region Ctor

        public WorkContext(IHttpContextAccessor httpContextAccessor, ITokenProvider tokenProvider)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _tokenProvider = tokenProvider;
        }

        #endregion

        #region Methods

        public string GetAccessToken()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = _httpContext.Request.Headers["Authorization"]
                    .ToString().Replace("Bearer ", string.Empty);
            }

            return _accessToken;
        }

        public string GetCurrentUserId()
        {
            try
            {
                if (string.IsNullOrEmpty(_userId))
                {
                    var token = GetAccessToken();
                    var userId = _tokenProvider.ExtractUserIdFromToken(token);
                    _userId = userId.ToString();
                }

                return _userId;
            }
            catch
            {
                return "";
            }

        }

        #endregion
    }
}
