using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Evenda.UI.Models.EventVM
{
    public class CreateEventVM
    {
        public Guid? EventId { get; set; }
        public bool IsInCreateMode { get; set; } = true;

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


        // Image related Props
        public IList<IFormFile> NewImages { get; set; } = [];
        public string? DeletedImageIds { get; set; } = "";
        public string? ThumbnailKey { get; set; }
        public IList<ImageVM> Images { get; set; } = [];
        public int? OriginalThumbnailImgIdx { get; set; }
    }

    public class ImageVM
    {
        public Guid? Id { get; set; }
        public string? ContentType { get; set; }
        public string? Base64 { get; set; }
    }
}
