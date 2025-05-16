using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Dtos;
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

        public async Task<PagedList<EventDto>> SendGetPaginatedFilteredEvents(PaginationDto pagination, EventFilterDto filterDto, bool includeThumbnailImg)
        {
            var formattedUrl = string.Format(ApiEndPoints.GET_FILTERED_EVENTS_PAGINATED, pagination.Page, pagination.PageSize, pagination.Sort, pagination.SortDir, includeThumbnailImg);
            var response = await PostAsync<PagedList<EventDto>, EventFilterDto>(formattedUrl, filterDto);
            return response.Data;
        }

        public async Task<EventDetailsDto> SendGetEventDetailsReq(Guid eventId)
        {
            var response = await GetAsync<EventDetailsDto>(string.Format(ApiEndPoints.GET_EVENT_DETAILS, eventId.ToString()));
            return response.Data;
        }

        public async Task<Guid> SendCreateEventReq(CreateEventDto createEventDto)
        {
            var response = await PostAsync<Guid, CreateEventDto>(ApiEndPoints.CREATE_EVENT, createEventDto);
            return response.Data;
        }

        public async Task SendCancelEventReq(Guid eventId)
        {
            var response = await DeleteAsync(string.Format(ApiEndPoints.CANCEL_EVENT, eventId.ToString()));
        }

        public async Task<IList<string>> GetCategories(bool inUseOnly = true)
        {
            var response = await GetAsync<IList<string>>(string.Format(ApiEndPoints.GET_CATEGORIES, inUseOnly));
            return response.Data;
        }

        #endregion
    }
}
