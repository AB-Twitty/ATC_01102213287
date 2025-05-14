using Evenda.UI.Exceptions;
using Evenda.UI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evenda.UI.Controllers
{
    public class DefaultController : Controller
    {
        protected async Task<T> ExecuteApiCall<T>(Func<Task<T>> apiCall, params HttpStatusCode[] throwOnStatusCodes)
        {
            try
            {
                return await apiCall();
            }
            catch (ApiException ex)
            {
                if (throwOnStatusCodes.Contains(ex.ApiResponse.StatusCode))
                {
                    throw;
                }

                switch (ex.ApiResponse.StatusCode)
                {
                    case HttpStatusCode.UnprocessableEntity:
                        ModelState.AddApiValidationErrors(ex.ApiResponse.Errors);
                        break;
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.NotFound:
                        ModelState.AddModelError(string.Empty, ex.ApiResponse.Message);
                        break;
                    default:
                        throw;
                }

                return default;
            }
        }

        protected async Task ExecuteApiCall(Func<Task> apiCall, params HttpStatusCode[] throwOnStatusCodes)
        {
            try
            {
                await apiCall();
            }
            catch (ApiException ex)
            {
                if (throwOnStatusCodes.Contains(ex.ApiResponse.StatusCode))
                {
                    throw;
                }

                switch (ex.ApiResponse.StatusCode)
                {
                    case HttpStatusCode.UnprocessableEntity:
                        ModelState.AddApiValidationErrors(ex.ApiResponse.Errors);
                        break;
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.NotFound:
                        ModelState.AddModelError(string.Empty, ex.ApiResponse.Message);
                        break;
                    default:
                        throw;
                }
            }
        }
    }
}
