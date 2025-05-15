namespace Evenda.UI.Dtos.Ticket
{
    public class BookingDto
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
    }
}
