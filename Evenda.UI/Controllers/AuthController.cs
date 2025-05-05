using Microsoft.AspNetCore.Mvc;

namespace Evenda.UI.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}
