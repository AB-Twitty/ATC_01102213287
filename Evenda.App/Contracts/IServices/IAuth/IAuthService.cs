using Evenda.App.Dtos.Auth;
using Evenda.App.Models;

namespace Evenda.App.Contracts.IServices.IAuth
{
    public interface IAuthService
    {
        Task<DataResponse<Guid>> Register(RegisterDto registerDto);
        Task<DataResponse<AuthDto>> Login(LoginDto loginDto);
        Task<DataResponse<AuthDto>> RefreshUserTokens(RefreshTokenDto refreshTokenDto);
        Task<BaseResponse> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task<BaseResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
