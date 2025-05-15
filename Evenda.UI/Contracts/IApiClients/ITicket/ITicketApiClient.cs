using Evenda.UI.Dtos.Ticket;
using Evenda.UI.Helpers;

namespace Evenda.UI.Contracts.IApiClients.ITicket
{
    public interface ITicketApiClient
    {
        Task<Guid> BookEventAsync(BookEventDto bookDto);
        Task<PagedList<BookingDto>> GetMyBookings(int page = 1, int pageSize = 15);
    }
}
