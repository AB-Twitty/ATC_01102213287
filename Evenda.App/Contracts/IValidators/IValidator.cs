using Evenda.App.Models.Validation;

namespace Evenda.App.Contracts.IValidators
{
    public abstract class IValidator<TDto>
    {
        protected void Required(ValidationResult validationResult, object? value, string propName)
        {
            bool isValid = true;

            if (value is string stringValue)
            {
                if (string.IsNullOrEmpty(stringValue)) isValid = false;
            }
            else
            {
                if (value == null) isValid = false;
            }

            if (!isValid)
            {
                validationResult.AddError(propName, $"{propName} is required.");
            }
        }

        protected void Range(ValidationResult validationResult, double value, string propName, double minValue, double maxValue)
        {
            if (value < minValue || value > maxValue)
            {
                validationResult.AddError(propName, $"{propName} must be between {minValue} and {maxValue}.");
            }
        }

        protected void Range(ValidationResult validationResult, decimal value, string propName, decimal minValue)
        {
            if (value < minValue)
            {
                validationResult.AddError(propName, $"{propName} must be greater than {minValue}.");
            }
        }

        protected void CustomValidation(ValidationResult validationResult, Func<TDto, bool> validationFunc, TDto obj, string errorMessage, string propName)
        {
            if (!validationFunc(obj))
            {
                validationResult.AddError(propName, errorMessage);
            }
        }

        public abstract ValidationResult Validate(TDto dto);
    }
}
