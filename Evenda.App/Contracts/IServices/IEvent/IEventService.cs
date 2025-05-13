using Evenda.App.Dtos.Event;
using Evenda.App.Models;
using Evenda.App.Utils;

namespace Evenda.App.Contracts.IServices.IEvent
{
    public interface IEventService
    {
        Task<DataResponse<PagedList<EventDto>>> GetEventsPaginated(int page, int pageSize);
        Task<DataResponse<EventDetailsDto>> GetEventDetails(Guid eventId);
        Task<DataResponse<Guid>> CreateEvent(CreateEventDto createDto);
    }
}
