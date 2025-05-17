using Evenda.App.Contracts.IValidators;
using Evenda.App.Dtos.Event;
using Evenda.App.Models.Validation;

namespace Evenda.App.Validators.EventValidators
{
    public class EventValidator : IValidator<CreateEventDto>
    {
        public override ValidationResult Validate(CreateEventDto dto)
        {
            var validationResult = new ValidationResult();

            Required(validationResult, dto.Name, nameof(dto.Name));
            Required(validationResult, dto.Description, nameof(dto.Description));
            Range(validationResult, dto.Price, nameof(dto.Price), 0);
            Required(validationResult, dto.Category, nameof(dto.Category));
            Required(validationResult, dto.Country, nameof(dto.Country));
            Required(validationResult, dto.City, nameof(dto.City));
            Required(validationResult, dto.Venue, nameof(dto.Venue));
            Required(validationResult, dto.DateTime, nameof(dto.DateTime));
            Range(validationResult, dto.TicketsQty, nameof(dto.TicketsQty), 1);

            return validationResult;
        }
    }
}
