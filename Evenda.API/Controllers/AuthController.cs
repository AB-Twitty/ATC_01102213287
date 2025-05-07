using Evenda.API.Controllers.Base;
using Evenda.App.Contracts.IServices.IAuth;
using Evenda.App.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Evenda.API.Controllers
{
    public class AuthController : DefaultController
    {
        #region Fields

        private readonly IAuthService _authService;

        #endregion

        #region Ctor

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #endregion

        #region Actions

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            return HandleResponse(await _authService.Login(loginDto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            return HandleResponse(await _authService.Register(registerDto));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            return HandleResponse(await _authService.RefreshUserTokens(refreshTokenDto));
        }

        #endregion
    }
}
