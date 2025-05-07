using Evenda.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Evenda.UI.Controllers
{
    public class DefaultController : Controller
    {
        protected async Task<T?> ExecuteApiCall<T>(Func<Task<T>> apiCall)
        {
            try
            {
                return await apiCall();
            }
            catch (ValidationException vex)
            {
                ModelState.AddApiValidationErrors(vex.Value);
                return default;
            }
        }

        protected async Task ExecuteApiCall(Func<Task> apiCall)
        {
            try
            {
                await apiCall();
            }
            catch (ValidationException vex)
            {
                ModelState.AddApiValidationErrors(vex.Value);
            }
        }
    }
}
