using Evenda.App.Dtos.Media;

namespace Evenda.App.Dtos.Event
{
    public class CreateEventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public IList<string> Tags { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public DateTime DateTime { get; set; }

        public int TicketsQty { get; set; }
        public string? ThumbnailKey { get; set; }
        public IList<FileUploadDto> Images { get; set; } = new List<FileUploadDto>();
    }
}
