using Evenda.UI.Contracts.IApiClients.IAuth;
using Evenda.UI.Contracts.IServices;
using Evenda.UI.Dtos.Auth;
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
        public IActionResult Login([FromQuery] string returnUrl = " ")
        {
            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            AuthDto authDto;

            try
            {
                authDto = await ExecuteApiCall(
                    () => _authApiClient.SendLoginReq(new LoginDto(loginVM)),
                    throwOnStatusCodes: [HttpStatusCode.Unauthorized]
                );

                if (!ModelState.IsValid) return View(loginVM);

            }
            catch (UnauthorizedAccessException uaex)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password.");
                return View(loginVM);
            }

            var claims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, authDto.Id.ToString()),
                        new(ClaimTypes.Name, authDto.FirstName),
                        new("lname", authDto.LastName),
                        new(ClaimTypes.Email, authDto.Email),
                        new(Constants.ACCESS_TOKEN_KEY, authDto.AccessToken),
                        new(Constants.REFRESH_TOKEN_KEY, authDto.RefreshToken)
                    };

            claims.AddRange(authDto.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, Constants.DEFAULT_AUTHENTICATION_SCHEME);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = loginVM.RememberMe,
                ExpiresUtc = loginVM.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(Constants.DEFAULT_AUTHENTICATION_SCHEME, new ClaimsPrincipal(identity), authProperties);

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
            await HttpContext.SignOutAsync(Constants.DEFAULT_AUTHENTICATION_SCHEME);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Forget Password

        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid) return View(forgotPasswordDto);

            await ExecuteApiCall(
                () => _authApiClient.SendForgotPasswordReq(forgotPasswordDto)
            );

            if (!ModelState.IsValid) return View(forgotPasswordDto);

            return ResetPassword(forgotPasswordDto.Email, true);
        }

        #endregion

        #region Reset Password

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string email, bool showEmailSentMsg = false)
        {
            ViewBag.ShowEmailSentMsg = showEmailSentMsg;
            return View("ResetPassword", new ResetPasswordDto { Email = email });
        }

        [HttpPost("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid) return View(resetPasswordDto);

            await ExecuteApiCall(
                () => _authApiClient.SendResetPasswordReq(resetPasswordDto)
            );

            if (!ModelState.IsValid) return View(resetPasswordDto);

            return RedirectToAction("Login");
        }

        #endregion

        #endregion
    }
}
