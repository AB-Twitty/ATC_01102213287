using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Dtos.Event;
using Evenda.UI.Models.EventVM;
using Microsoft.AspNetCore.Mvc;

namespace Evenda.UI.Controllers
{
    public class EventsController : DefaultController
    {
        #region Fields

        private readonly IEventApiCLient _eventApiClient;

        #endregion

        #region Ctor

        public EventsController(IEventApiCLient eventApiClient)
        {
            _eventApiClient = eventApiClient;
        }

        #endregion

        #region Actions

        [HttpGet("events")]
        public async Task<IActionResult> Index([FromQuery] int pg = 1, [FromQuery] EventFilterDto? filterDto = null)
        {
            var dataList = await ExecuteApiCall(() => _eventApiClient.SendGetPaginatedEventsReq(pg, 4));
            return View(new EventCardsListVM { Events = dataList, Filter = filterDto });
        }

        [HttpGet("event/{name}/{id}")]
        public async Task<IActionResult> Details(string name, Guid id)
        {
            var data = await ExecuteApiCall(() => _eventApiClient.SendGetEventDetailsReq(id));
            return View(data);
        }

        [HttpPost("event/filter")]
        public async Task<IActionResult> Filter(EventFilterDto filterDto)
        {
            return RedirectToAction("Index", filterDto);
        }

        #endregion
    }
}
