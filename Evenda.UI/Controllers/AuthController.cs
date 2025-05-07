using Evenda.UI.Contracts.IApiClients.IAuth;
using Evenda.UI.Contracts.IServices;
using Evenda.UI.Dtos.Auth;
using Evenda.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Evenda.UI.Controllers
{
    [Route("auth")]
    public class AuthController : DefaultController
    {
        #region Fields

        private readonly IAuthApiClient _authApiClient;
        private readonly IApiTokenService _apiTokenService;

        #endregion

        #region Ctor

        public AuthController(IAuthApiClient authApiClient, IApiTokenService apiTokenService)
        {
            _authApiClient = authApiClient;
            _apiTokenService = apiTokenService;
        }

        #endregion

        #region Actions

        #region Login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            try
            {
                var authDto = await _authApiClient.SendLoginReq(loginDto);
                _apiTokenService.SetTokens(authDto.AccessToken, authDto.RefreshToken);
            }
            catch (ValidationException vex)
            {
                ModelState.AddApiValidationErrors(vex.Value);
                return View(loginDto);
            }
            catch (UnauthorizedAccessException uaex)
            {
                ModelState.AddModelError(string.Empty, uaex.Message);
                return View(loginDto);
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Register
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            await ExecuteApiCall(async () => await _authApiClient.SendRegisterReq(registerDto));

            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            return RedirectToAction("Login", new LoginDto { Email = registerDto.Email });
        }
        #endregion

        #endregion
    }
}
