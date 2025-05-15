using Evenda.UI.Dtos.Media;

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

        public int TicketsQuantity { get; set; }
        public int BookedTickets { get; set; }
        public int AvailableTickets => TicketsQuantity - BookedTickets;
        public bool? IsBooked { get; set; }

        public FileUploadDto Image { get; set; }
        public IList<string> Tags { get; set; } = new List<string>();
    }
}
