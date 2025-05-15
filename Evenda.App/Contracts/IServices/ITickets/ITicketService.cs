using Evenda.App.Dtos.Tickets;
using Evenda.App.Models;

namespace Evenda.App.Contracts.IServices.ITickets
{
    public interface ITicketService
    {
        Task<DataResponse<Guid>> BookEvent(BookEventDto bookDto);
    }
}
