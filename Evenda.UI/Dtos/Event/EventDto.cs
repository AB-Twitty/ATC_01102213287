namespace Evenda.UI.Dtos.Event
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Venue { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public DateTime DateTime { get; set; }

        public string? ImagePath { get; set; }
        public IList<ImageDto> Images { get; set; }
    }

    public class ImageDto
    {
        public string Path { get; set; }
        public bool IsThumbnail { get; set; }
    }
}
