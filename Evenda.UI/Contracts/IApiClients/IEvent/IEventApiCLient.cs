using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;

namespace Evenda.UI.Contracts.IApiClients.IEvent
{
    public interface IEventApiCLient
    {
        Task<PagedList<EventDto>> SendGetPaginatedEventsReq(int page, int pageSize);
        Task<EventDetailsDto> SendGetEventDetailsReq(Guid eventId);
    }
}
