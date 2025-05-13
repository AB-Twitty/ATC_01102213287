using Evenda.App.Contracts.IValidators;
using Evenda.App.Dtos.Auth;
using Evenda.App.Models.Validation;
using Evenda.App.Utils.Constants;

namespace Evenda.App.Validators.AuthValidators
{
    public class LoginDtoValidator : IValidator<LoginDto>
    {
        public override ValidationResult Validate(LoginDto dto)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                result.AddError("Email", "Email is required");
            }

            if (!string.IsNullOrWhiteSpace(dto.Email) && !CustomRegex.EmailRegex().IsMatch(dto.Email))
            {
                result.AddError("Email", "Email is not valid");
            }

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                result.AddError("Password", "Password is required");
            }

            return result;
        }
    }
}
