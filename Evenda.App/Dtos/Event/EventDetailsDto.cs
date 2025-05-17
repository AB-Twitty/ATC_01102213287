using Evenda.App.Dtos.Media;

using EventEntity = Evenda.Domain.Entities.EventEntities.Event;

namespace Evenda.App.Dtos.Event
{
    public class EventDetailsDto
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

        public IList<ImageDto> Images { get; set; }
        public IList<string> Tags { get; set; }

        public EventDetailsDto()
        {

        }

        public EventDetailsDto(EventEntity @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Description = @event.Description;
            Venue = @event.Venue;
            Country = @event.Country;
            City = @event.City;
            Price = @event.Price;
            Category = @event.Category;
            DateTime = @event.DateTime;

            TicketsQuantity = @event.TicketsQuantity;

            Images = @event.Images?.OrderByDescending(i => i.IsThumbnail)
                .Select(img => new ImageDto { Id = img.Id, IsThumbnail = img.IsThumbnail, File = new FileUploadDto(img) })
                .ToList() ?? new List<ImageDto>();

            Tags = @event?.Tags?.Select(x => x.Name).ToList() ?? new List<string>();
        }
    }
}
