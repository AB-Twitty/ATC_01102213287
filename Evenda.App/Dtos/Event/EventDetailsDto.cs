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

        public IList<FileUploadDto> Images { get; set; }

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

            Images = @event.Images?.OrderByDescending(i => i.IsThumbnail)
                .Select(img => new FileUploadDto(img)).ToList() ?? new List<FileUploadDto>();
        }
    }
}
