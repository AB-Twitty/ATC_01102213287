using Evenda.App.Models.Validation;

namespace Evenda.App.Contracts.IValidators
{
    public interface IValidatorDispatcher
    {
        ValidationResult Validate<T>(T Dto);
    }
}
