using Evenda.Domain.Base;
using Evenda.Domain.Entities.EventEntities;
using Evenda.Domain.Entities.UserEntities;

namespace Evenda.Domain.Entities.TicketEntities
{
    public class Ticket : BaseEntity
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
