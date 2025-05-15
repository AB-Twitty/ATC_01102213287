using Evenda.UI.Contracts.IApiClients.ITicket;
using Evenda.UI.Dtos.Ticket;
using Evenda.UI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Evenda.UI.Controllers
{
    public class TicketsController : DefaultController
    {
        #region Fields

        private readonly ITicketApiClient _ticketApiClient;

        #endregion

        #region Ctor

        public TicketsController(ITicketApiClient ticketApiClient)
        {
            _ticketApiClient = ticketApiClient;
        }

        #endregion

        #region Actions

        [HttpPost("book")]
        [Authorize(Roles = Constants.CUSTOMER_ROLE_NAME)]
        public async Task<IActionResult> BookEvent(BookEventDto bookDto, string eventName)
        {
            var result = await ExecuteApiCall(
                () => _ticketApiClient.BookEventAsync(bookDto),
                throwOnStatusCodes: [HttpStatusCode.NotFound]
            );

            TempData["success"] = false;
            if (ModelState.IsValid) TempData["success"] = true;

            TempData["BookingMsg"] = ModelState.IsValid
                ? "Booking was successful, you will find your ticket in the booking list."
                : ModelState.Where(x => x.Key == string.Empty)
                            .Select(e => e.Value?.Errors.FirstOrDefault()?.ErrorMessage)
                            .FirstOrDefault();

            return RedirectToAction("Details", "Events", new { Name = eventName.Replace(' ', '-'), Id = bookDto.EventId });
        }

        [HttpGet("my-bookings")]
        [Authorize(Roles = Constants.CUSTOMER_ROLE_NAME)]
        public async Task<IActionResult> Index([FromQuery] int pg = 1)
        {
            var tickets = await ExecuteApiCall(
                () => _ticketApiClient.GetMyBookings(pg, pageSize: 15)
            );

            return View(tickets);
        }

        #endregion
    }
}
