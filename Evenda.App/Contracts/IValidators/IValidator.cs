using Evenda.App.Models.Validation;

namespace Evenda.App.Contracts.IValidators
{
    public interface IValidator<TDto>
    {
        ValidationResult Validate(TDto dto);
    }
}
