using Evenda.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evenda.API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class DefaultController : ControllerBase
    {
        protected virtual IActionResult HandleResponse(BaseResponse response)
        {
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
