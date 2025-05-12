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
                TagsSelectItems = tags.OrderByDescending(t => t.EventsCnt)
                    .Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.Id.ToString(),
                        Selected = filterDto?.TagIds.Contains(t.Id) ?? false
                    }),
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

        [HttpGet("dashboard/events")]
        public async Task<IActionResult> DashboardList()
        {
            return View();
        }

        [HttpPost("dashboard/events")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DashboardList(string[] status)
        {
            return await DashboardList();
        }

        #region Create Event
        [HttpGet("dashboard/events/new")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("dashboard/events/new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventVM createVM)
        {
            if (!ModelState.IsValid)
                return View(createVM);

            var createDto = new CreateEventDto(createVM);

            var eventId = await ExecuteApiCall(() => _eventApiClient.SendCreateEventReq(createDto));

            return RedirectToAction(nameof(Details), new { id = eventId, name = createVM.Name });
        }
        #endregion

        #endregion
    }
}
