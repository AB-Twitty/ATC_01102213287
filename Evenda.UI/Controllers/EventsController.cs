using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Contracts.IApiClients.ITag;
using Evenda.UI.Dtos.Event;
using Evenda.UI.Models.EventVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evenda.UI.Controllers
{
    public class EventsController : DefaultController
    {
        #region Fields

        private readonly IEventApiCLient _eventApiClient;
        private readonly ITagApiClient _tagApiClient;

        #endregion

        #region Ctor

        public EventsController(IEventApiCLient eventApiClient, ITagApiClient tagApiClient)
        {
            _eventApiClient = eventApiClient;
            _tagApiClient = tagApiClient;
        }

        #endregion

        #region Actions

        [HttpGet("events")]
        public async Task<IActionResult> Index([FromQuery] int pg = 1, [FromQuery] EventFilterDto? filterDto = null)
        {
            var events = await ExecuteApiCall(() => _eventApiClient.SendGetPaginatedEventsReq(pg, 8));

            var tags = await ExecuteApiCall(() => _tagApiClient.SendGetTagsInUseReq());

            var model = new EventCardsListVM
            {
                Events = events,
                TagsSelectItems = tags.OrderByDescending(t => t.EventsCnt).Select(t => new SelectListItem { Text = t.Name, Value = t.Id.ToString() }),
                Filter = filterDto ?? new EventFilterDto()
            };

            return View(model);
        }

        [HttpGet("event/{name}/{id}")]
        public async Task<IActionResult> Details(string name, Guid id)
        {
            var data = await ExecuteApiCall(() => _eventApiClient.SendGetEventDetailsReq(id));
            return View(data);
        }

        #endregion
    }
}
