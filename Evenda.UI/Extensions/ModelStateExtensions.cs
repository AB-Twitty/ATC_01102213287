using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Evenda.UI.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddApiValidationErrors(this ModelStateDictionary modelState, object? errors)
        {
            if (errors is not IDictionary<string, IList<string>> validationErrors)
                return;

            foreach (var errorList in validationErrors.Values)
            {
                foreach (var error in errorList)
                {
                    modelState.AddModelError(string.Empty, error);
                }
            }
        }
    }
}
