using Evenda.App.Contracts.IValidators;
using Evenda.App.Models.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Evenda.App.Validators
{
    public class ValidatorDispatcher : IValidatorDispatcher
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Ctor

        public ValidatorDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods

        public ValidationResult Validate<T>(T input)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();
            if (validator == null)
                return new ValidationResult();

            return validator.Validate(input);
        }

        #endregion
    }
}
