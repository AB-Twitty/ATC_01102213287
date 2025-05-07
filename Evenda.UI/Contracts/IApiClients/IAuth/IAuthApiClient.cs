using Evenda.UI.Dtos.Auth;

namespace Evenda.UI.Contracts.IApiClients.IAuth
{
    public interface IAuthApiClient
    {
        Task<AuthDto> SendLoginReq(LoginDto loginDto);
        Task SendRegisterReq(RegisterDto registerDto);
    }
}
