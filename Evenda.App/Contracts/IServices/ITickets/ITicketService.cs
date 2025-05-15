using Evenda.App.Dtos.Tickets;
using Evenda.App.Models;
using Evenda.App.Utils;

namespace Evenda.App.Contracts.IServices.ITickets
{
    public interface ITicketService
    {
        Task<DataResponse<Guid>> BookEvent(BookEventDto bookDto);
        Task<DataResponse<PagedList<TicketDto>>> GetUserBookings(PaginationModel pagination);
    }
}
