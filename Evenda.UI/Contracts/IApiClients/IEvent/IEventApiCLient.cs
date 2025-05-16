using Evenda.UI.Dtos;
using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;

namespace Evenda.UI.Contracts.IApiClients.IEvent
{
    public interface IEventApiCLient
    {
        Task<PagedList<EventDto>> SendGetPaginatedEventsReq(int page, int pageSize);
        Task<PagedList<EventDto>> SendGetPaginatedFilteredEvents(PaginationDto pagination, EventFilterDto filterDto, bool includeThumbnailImg);
        Task<EventDetailsDto> SendGetEventDetailsReq(Guid eventId);
        Task<Guid> SendCreateEventReq(CreateEventDto createEventDto);
        Task SendCancelEventReq(Guid eventId);
        Task<IList<string>> GetCategories(bool inUseOnly = true);
    }
}
