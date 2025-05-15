using Evenda.Domain.Base;
using Evenda.Domain.Entities.MediaEntities;
using Evenda.Domain.Entities.TagEntities;
using Evenda.Domain.Entities.TicketEntities;

namespace Evenda.Domain.Entities.EventEntities
{
    public class Event : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public DateTime DateTime { get; set; }
        public int TicketsQuantity { get; set; }

        public virtual ICollection<Image> Images { get; set; } = new HashSet<Image>();
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
