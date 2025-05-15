using Evenda.Domain.Entities.TicketEntities;

namespace Evenda.App.Dtos.Tickets
{
    public class TicketDto
    {
        public Guid TicketId { get; set; }

        public Guid EventId { get; set; }
        public string EventName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime TicketBookedAt { get; set; }
        public bool IsCanceled { get; set; }

        public string BookingStatus { get; set; }

        public TicketDto(Ticket ticket)
        {
            TicketId = ticket.Id;

            EventId = ticket.EventId;
            EventName = ticket.Event.Name;
            Category = ticket.Event.Category;
            Price = ticket.Event.Price;
            EventDate = ticket.Event.DateTime;

            TicketBookedAt = ticket.DateCreated;
            IsCanceled = ticket.IsDeleted;

            if (ticket.IsDeleted)
            {
                BookingStatus = "Cancelled";
            }
            else if (ticket.Event.DateTime < DateTime.UtcNow)
            {
                BookingStatus = "Completed";
            }
            else
            {
                BookingStatus = "Upcoming";
            }
        }
    }
}
