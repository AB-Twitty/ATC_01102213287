using Evenda.UI.Contracts.IApiClients.IAuth;
using Evenda.UI.Dtos.Auth;
using Evenda.UI.Helpers;

namespace Evenda.UI.ApiClients.Auth
{
    public class AuthApiClient : ApiClient, IAuthApiClient
    {
        public AuthApiClient(IHttpClientFactory httpClient) : base(httpClient)
        {

        }

        public async Task<AuthDto> SendLoginReq(LoginDto loginDto)
        {
            var response = await PostAsync<AuthDto, LoginDto>(ApiEndPoints.LOGIN, loginDto);
            return response.Data;
        }

        public async Task SendRegisterReq(RegisterDto registerDto)
        {
            var response = await PostAsync(ApiEndPoints.REGISTER, registerDto);
        }
    }
}
