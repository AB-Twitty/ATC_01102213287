using Evenda.API.Controllers.Base;
using Evenda.App.Contracts.IServices.IEvent;
using Evenda.App.Dtos.Event;
using Evenda.App.Models;
using Evenda.App.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            var response = await _eventService.CreateEvent(createEventDto);
            return HandleResponse(response);
        }

        [HttpDelete("cancel/{eventId}")]
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        public async Task<IActionResult> CancelEvent([FromRoute] Guid eventId)
        {
            var response = await _eventService.CancelEvent(eventId);
            return HandleResponse(response);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories([FromQuery(Name = "in-use")] bool inUseOnly = true)
        {
            var response = await _eventService.GetCategories(inUseOnly);
            return HandleResponse(response);
        }
        #endregion
    }
}
