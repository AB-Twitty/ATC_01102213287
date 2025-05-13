using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Contracts.IApiClients.ITag;
using Evenda.UI.Contracts.IHelper;
using Evenda.UI.Dtos;
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
        private readonly IDropdownHelper _dropdownHelper;

        #endregion

        #region Ctor

        public EventsController(IEventApiCLient eventApiClient, ITagApiClient tagApiClient, IDropdownHelper dropdownHelper)
        {
            _eventApiClient = eventApiClient;
            _tagApiClient = tagApiClient;
            _dropdownHelper = dropdownHelper;
        }

        #endregion

        #region Actions

        [HttpGet("events")]
        public async Task<IActionResult> Index([FromQuery] int pg = 1, [FromQuery] EventFilterDto? filterDto = null)
        {
            var events = await ExecuteApiCall(() => _eventApiClient.SendGetPaginatedEventsReq(pg, 12));

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
                Filter = filterDto
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
        public async Task<IActionResult> DashboardList([FromQuery] EventFilterVM filterVM, [FromQuery] int pg = 1)
        {
            var events = await ExecuteApiCall(() => _eventApiClient.SendGetPaginatedFilteredEvents(new PaginationDto
            {
                Page = pg,
                PageSize = filterVM.TableSize,
                Sort = filterVM.Sort,
                SortDir = filterVM.SortDir
            }, new EventFilterDto(filterVM)));

            var model = new DashboardEventListVM
            {
                Filter = filterVM,
                Events = events
            };

            return View(model);
        }

        #region Create Event
        [HttpGet("dashboard/events/new")]
        public async Task<IActionResult> Create()
        {
            var tags = await ExecuteApiCall(() => _tagApiClient.SendGetTagsInUseReq());
            var tagsSelectItems = tags.OrderByDescending(t => t.EventsCnt)
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id.ToString(),
                    Selected = false
                });

            return View(new CreateEventVM { TagsSelectItems = tagsSelectItems });
        }

        [HttpPost("dashboard/events/new")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventVM createVM)
        {
            if (!ModelState.IsValid)
            {
                var tags = await ExecuteApiCall(() => _tagApiClient.SendGetTagsInUseReq());
                createVM.TagsSelectItems = tags.OrderByDescending(t => t.EventsCnt)
                    .Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.Id.ToString(),
                        Selected = createVM.StringTags?.Split(',').Contains(t.Id.ToString()) ?? false
                    })
                    .Union(createVM.StringTags?.Split(',').Select(x => new SelectListItem
                    {
                        Text = x,
                        Value = x,
                        Selected = true
                    }) ?? [])
                    .GroupBy(i => i.Value).SelectMany(g => g.Take(1));
                return View(createVM);
            }

            var createDto = new CreateEventDto(createVM);
            await createDto.ReadImagesFromFiles(createVM.Images);

            var eventId = await ExecuteApiCall(() => _eventApiClient.SendCreateEventReq(createDto));

            return RedirectToAction(nameof(Details), new { id = eventId, name = createVM.Name });
        }
        #endregion

        #endregion
    }
}
