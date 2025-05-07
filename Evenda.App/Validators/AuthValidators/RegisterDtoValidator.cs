using Evenda.App.Contracts.IValidators;
using Evenda.App.Dtos.Auth;
using Evenda.App.Models.Validation;
using Evenda.App.Utils.Constants;

namespace Evenda.App.Validators.AuthValidators
{
    public class RegisterDtoValidator : IValidator<RegisterDto>
    {
        public ValidationResult Validate(RegisterDto dto)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(dto.FirstName))
            {
                result.AddError("FirstName", "First name is required.");
            }
            if (string.IsNullOrEmpty(dto.LastName))
            {
                result.AddError("LastName", "Last name is required.");
            }
            if (string.IsNullOrEmpty(dto.Email))
            {
                result.AddError("Email", "Email is required.");
            }
            else if (!CustomRegex.EmailRegex().IsMatch(dto.Email))
            {
                result.AddError("Email", "Email is not valid.");
            }
            if (string.IsNullOrEmpty(dto.Password))
            {
                result.AddError("Password", "Password is required.");
            }
            else if (!CustomRegex.PasswordRegex().IsMatch(dto.Password))
            {
                result.AddError("Password", "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
            }

            return result;
        }
    }
}
