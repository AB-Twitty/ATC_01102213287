using Evenda.API.Controllers.Base;
using Evenda.App.Contracts.IServices.IEvent;
using Evenda.App.Dtos.Event;
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
        public async Task<IActionResult> GetEventsPaginated([FromQuery] int page, int pageSize = 12)
        {
            var response = await _eventService.GetEventsPaginated(page, pageSize);
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
