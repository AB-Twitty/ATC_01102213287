using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;

namespace Evenda.UI.ApiClients.Event
{
    public class EventApiClient : ApiClient, IEventApiCLient
    {
        #region Fields

        #endregion

        #region Ctor

        public EventApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }

        #endregion

        #region Requests

        public async Task<PagedList<EventDto>> SendGetPaginatedEventsReq(int page, int pageSize)
        {
            var response = await GetAsync<PagedList<EventDto>>(string.Format(ApiEndPoints.GET_PAGINATED_EVENTS, page, pageSize));
            return response.Data;
        }

        public async Task<EventDetailsDto> SendGetEventDetailsReq(Guid eventId)
        {
            var response = await GetAsync<EventDetailsDto>(string.Format(ApiEndPoints.GET_EVENT_DETAILS, eventId.ToString()));
            return response.Data;
        }

        public async Task<Guid> SendCreateEventReq(CreateEventDto createEventDto)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
