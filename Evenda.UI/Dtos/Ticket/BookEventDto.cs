namespace Evenda.UI.Dtos.Ticket
{
    public class BookEventDto
    {
        public Guid EventId { get; set; }
        public BookEventDto(Guid eventId)
        {
            EventId = eventId;
        }
        public BookEventDto()
        {
        }
    }
}
