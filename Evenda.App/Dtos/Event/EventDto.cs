using Evenda.App.Dtos.Media;
using EventEntity = Evenda.Domain.Entities.EventEntities.Event;

namespace Evenda.App.Dtos.Event
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

        public FileUploadDto? Image { get; set; }

        public IList<string> Tags { get; set; }


        public EventDto() { }

        public EventDto(EventEntity @event)
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

            Tags = @event?.Tags?.Select(x => x.Name).ToList() ?? new List<string>();
        }
    }
}
