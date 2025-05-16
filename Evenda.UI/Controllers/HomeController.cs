using Evenda.UI.Contracts.IApiClients.IEvent;
using Evenda.UI.Dtos;
using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;
using Evenda.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evenda.UI.Controllers
{
    public class HomeController : DefaultController
    {
        #region Fields

        private readonly IEventApiCLient _eventApiClient;

        #endregion

        #region Ctor

        public HomeController(IEventApiCLient eventApiClient)
        {
            _eventApiClient = eventApiClient;
        }

        #endregion

        #region Actions

        [HttpGet]
        [Authorize(Roles = Constants.ADMIN_ROLE_NAME)]
        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var latestEvents = await ExecuteApiCall(() => _eventApiClient.SendGetPaginatedFilteredEvents(
                pagination: new PaginationDto { Page = 1, PageSize = 4, Sort = SortColumns.Latest, SortDir = "desc" },
                new EventFilterDto(), true
            ));

            var upcomingEvents = await ExecuteApiCall(() => _eventApiClient.SendGetPaginatedFilteredEvents(
                pagination: new PaginationDto { Page = 1, PageSize = 4, Sort = SortColumns.DateTime, SortDir = "asc" },
                new EventFilterDto(), true
            ));

            var mostBooked = await ExecuteApiCall(() => _eventApiClient.SendGetPaginatedFilteredEvents(
                pagination: new PaginationDto { Page = 1, PageSize = 4, Sort = SortColumns.Booked, SortDir = "desc" },
                new EventFilterDto(), true
            ));

            var model = new HomeVM
            {
                LatestEvents = latestEvents.Items,
                UpcomingEvents = upcomingEvents.Items,
                MostBookedUpcomingEvents = mostBooked.Items
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
