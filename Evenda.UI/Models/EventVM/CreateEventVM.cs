using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Evenda.UI.Models.EventVM
{
    public class CreateEventVM
    {
        public IEnumerable<SelectListItem> TagsSelectItems { get; set; }


        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        public string? StringTags { get; set; }

        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Venue { get; set; }
        [DisplayName("Tickets Quantity")]
        [Required]
        [Range(1, Int64.MaxValue, ErrorMessage = "Number of tickets can not be less than 1.")]
        public int TicketsQty { get; set; }
        [DisplayName("Event Date")]
        [Required]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        [DisplayName("Event Time")]
        [Required]
        public TimeOnly Time { get; set; } = TimeOnly.FromDateTime(DateTime.Now.AddHours(1));


        public IList<IFormFile> Images { get; set; } = new List<IFormFile>();

        public int ThumbnailIdx { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ThumbnailIdx < 0 || ThumbnailIdx >= Images.Count)
            {
                yield return new ValidationResult(
                    "Thumbnail index must be within the bounds of the Images array.",
                    new[] { nameof(ThumbnailIdx) });
            }
        }
    }
}
