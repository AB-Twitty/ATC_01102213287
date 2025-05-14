using Evenda.API.Controllers.Base;
using Evenda.App.Contracts.IServices.IEvent;
using Evenda.App.Dtos.Event;
using Evenda.App.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evenda.API.Controllers
{
    public class EventsController : DefaultController
    {
        #region Fields

        private readonly IEventService _eventService;

        #endregion

        #region Ctor

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        #endregion

        #region Actions

        [HttpGet("paginated")]
        public async Task<IActionResult> GetEventsPaginated([FromQuery] PaginationModel pagination,
            [FromBody] EventFilterDto filterDto)
        {
            var response = await _eventService.GetEventsPaginated(pagination.Page, pagination.PageSize);
            return HandleResponse(response);
        }

        [HttpPost("filter/paginated")]
        public async Task<IActionResult> GetFilteredEventsPaginated([FromQuery] PaginationModel pagination,
            [FromBody] EventFilterDto filterDto, [FromQuery(Name = "include-thumbnail")] bool includeThumbnailImg = false)
        {
            var response = await _eventService.GetFilteredEventsPaginated(pagination, filterDto, includeThumbnailImg);
            return HandleResponse(response);
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventById([FromRoute] Guid eventId)
        {
            var response = await _eventService.GetEventDetails(eventId);
            return HandleResponse(response);
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            var response = await _eventService.CreateEvent(createEventDto);
            return HandleResponse(response);
        }
        #endregion
    }
}
