﻿using Evenda.UI.Dtos.Auth;

namespace Evenda.UI.Contracts.IApiClients.IAuth
{
    public interface IAuthApiClient
    {
        Task<AuthDto> SendLoginReq(LoginDto loginDto);
        Task SendRegisterReq(RegisterDto registerDto);
        Task<AuthDto> SendRefreshTokenReq(RefreshTokenDto refreshTokenDto);
        Task SendForgotPasswordReq(ForgotPasswordDto forgotPasswordDto);
        Task SendResetPasswordReq(ResetPasswordDto resetPasswordDto);
    }
}
