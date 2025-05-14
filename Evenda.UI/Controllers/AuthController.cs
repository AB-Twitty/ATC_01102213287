using Evenda.UI.Contracts.IApiClients.IAuth;
using Evenda.UI.Contracts.IServices;
using Evenda.UI.Dtos.Auth;
using Evenda.UI.Exceptions;
using Evenda.UI.Extensions;
using Evenda.UI.Helpers;
using Evenda.UI.Models.AuthVM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

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
        public IActionResult Login([FromQuery] string returnUrl = "")
        {
            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            AuthDto authDto;

            try
            {
                authDto = await ExecuteApiCall(
                    () => _authApiClient.SendLoginReq(new LoginDto(loginVM)),
                    throwOnStatusCodes: [HttpStatusCode.Unauthorized]
                );

            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(loginVM);
            }

            HttpContext.Session.SetUserSession(authDto);

            if (loginVM.RememberMe)
            {
                var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, authDto.Id.ToString()),
                        new(ClaimTypes.Name, authDto.FirstName),
                        new(ClaimTypes.Email, authDto.Email)
                    };

                claims.AddRange(authDto.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var identity = new ClaimsIdentity(claims, Constants.DEFAULT_AUTHENTICATION_SCHEME);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = loginVM.RememberMe,
                    ExpiresUtc = loginVM.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(Constants.DEFAULT_AUTHENTICATION_SCHEME, new ClaimsPrincipal(identity), authProperties);

            }

            _apiTokenService.SetTokens(authDto.AccessToken, authDto.RefreshToken);


            return !string.IsNullOrEmpty(loginVM.ReturnUrl)
               ? Redirect(loginVM.ReturnUrl)
               : RedirectToAction("Index", "Home");
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

            return RedirectToAction("Login", new LoginVM { Email = registerDto.Email });
        }
        #endregion

        #region Logout
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(Constants.DEFAULT_AUTHENTICATION_SCHEME);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #endregion
    }
}
