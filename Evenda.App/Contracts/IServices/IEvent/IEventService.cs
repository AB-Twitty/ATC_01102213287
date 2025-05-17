using Evenda.App.Dtos.Event;
using Evenda.App.Models;
using Evenda.App.Utils;

namespace Evenda.App.Contracts.IServices.IEvent
{
    public interface IEventService
    {
        Task<DataResponse<PagedList<EventDto>>> GetEventsPaginated(int page, int pageSize, bool includeThumbnailImg = false);
        Task<DataResponse<PagedList<EventDto>>> GetFilteredEventsPaginated(PaginationModel pagination, EventFilterDto filterDto, bool includeThumbnailImg = false);
        Task<DataResponse<EventDetailsDto>> GetEventDetails(Guid eventId);
        Task<DataResponse<Guid>> CreateEvent(CreateEventDto createDto);
        Task<DataResponse<Guid>> EditEvent(EditEventDto editDto);
        Task<BaseResponse> CancelEvent(Guid eventId);
        Task<DataResponse<IList<string>>> GetCategories(bool inUseOnly = true);
    }
}
