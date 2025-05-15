using Evenda.UI.Dtos.Ticket;

namespace Evenda.UI.Contracts.IApiClients.ITicket
{
    public interface ITicketApiClient
    {
        Task<Guid> BookEventAsync(BookEventDto bookDto);
    }
}
