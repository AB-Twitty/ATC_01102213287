using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Contracts.IApiClients.ITag;
using Evenda.UI.Contracts.IHelper;
using Evenda.UI.Dtos;
using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;
using Evenda.UI.Models.EventVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        #region Utils

        protected virtual async Task<EventsListVM> CreateEventListViewModel(
            EventFilterVM filterVM,
            int pg = 1,
            int sz = 12,
            bool includeThumbnail = false)
        {
            var events = await ExecuteApiCall(() => _eventApiClient.SendGetPaginatedFilteredEvents(new PaginationDto
            {
                Page = pg,
                PageSize = sz,
                Sort = filterVM.Sort,
                SortDir = filterVM.SortDir
            }, new EventFilterDto(filterVM), true));

            return new EventsListVM { Filter = filterVM, Events = events };
        }

        #endregion

        #region Actions

        #region Event List
        [HttpGet("events")]
        public async Task<IActionResult> Index([FromQuery] EventFilterVM filterVM, [FromQuery] int pg = 1)
        {
            var model = await CreateEventListViewModel(filterVM, pg, 12, true);
            return View(model);
        }

        [HttpGet("dashboard/events")]
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        public async Task<IActionResult> DashboardList([FromQuery] EventFilterVM filterVM, [FromQuery] int pg = 1)
        {
            var model = await CreateEventListViewModel(filterVM, pg, filterVM.TableSize, true);
            return View(model);
        }
        #endregion

        #region Event Details
        [HttpGet("event/{name}/{id}")]
        public async Task<IActionResult> Details(string name, Guid id)
        {
            var data = await ExecuteApiCall(() => _eventApiClient.SendGetEventDetailsReq(id));
            return View(data);
        }
        #endregion

        #region Create Event
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        [HttpGet("dashboard/events/new")]
        public IActionResult Create()
        {
            return View(new CreateEventVM());
        }

        [HttpPost("dashboard/events/new")]
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventVM createVM)
        {
            if (!ModelState.IsValid) return View(createVM);

            var dto = new CreateEventDto(createVM);
            await dto.ReadImagesFromFiles(createVM.NewImages);

            var eventId = await ExecuteApiCall(() => _eventApiClient.SendCreateEventReq(dto));
            return RedirectToAction(nameof(Details), new
            {
                id = eventId,
                name = createVM.Name.Replace(' ', '-')
            });
        }
        #endregion

        #region Cancel Event
        [HttpPost("cancel")]
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelEvent(Guid eventId)
        {
            await ExecuteApiCall(() => _eventApiClient.SendCancelEventReq(eventId));
            TempData["success"] = true;

            return RedirectToAction("DashboardList");
        }
        #endregion

        #region Edit Event
        [HttpGet("edit/{id}")]
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        public async Task<IActionResult> EditEvent(Guid id)
        {
            var @event = await ExecuteApiCall(() => _eventApiClient.SendGetEventDetailsReq(id));

            var model = new CreateEventVM
            {
                IsInCreateMode = false,
                EventId = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                Price = @event.Price,
                Category = @event.Category,
                StringTags = string.Join(',', @event.Tags),
                Country = @event.Country,
                City = @event.City,
                Venue = @event.Venue,
                Date = DateOnly.FromDateTime(@event.DateTime),
                Time = TimeOnly.FromDateTime(@event.DateTime),
                TicketsQty = @event.TicketsQuantity,
                ThumbnailKey = @event.Images.FirstOrDefault(x => x.IsThumbnail)?.Id.ToString() ?? "",
                Images = @event.Images.Select(x => new ImageVM
                {
                    Id = x.Id,
                    ContentType = x.File.ContentType,
                    Base64 = Convert.ToBase64String(x.File.FileStream)
                }).ToList(),
                OriginalThumbnailImgIdx = @event.Images.IndexOf(@event.Images.First(x => x.IsThumbnail)),
            };

            return View("Create", model);
        }

        [HttpPost("edit/{id}")]
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(CreateEventVM createVM)
        {
            if (!ModelState.IsValid)
            {
                createVM.DeletedImageIds = "";
                createVM.IsInCreateMode = false;
                return View("Create", createVM);
            }

            var dto = new EditEventDto(createVM);
            await dto.ReadImagesFromFiles(createVM.NewImages);

            var eventId = await ExecuteApiCall(() => _eventApiClient.SendEditEventReq(dto));
            return RedirectToAction(nameof(Details), new
            {
                id = eventId,
                name = createVM.Name.Replace(' ', '-')
            });
        }
        #endregion

        #endregion
    }
}
