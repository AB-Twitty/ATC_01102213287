using Evenda.API.Controllers.Base;
using Evenda.App.Contracts.IServices.ITickets;
using Evenda.App.Dtos.Tickets;
using Evenda.App.Models;
using Evenda.App.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evenda.API.Controllers
{
    public class TicketsController : DefaultController
    {
        #region Fields

        private readonly ITicketService _ticketService;

        #endregion

        #region Ctor

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        #endregion

        #region Actions

        [HttpPost("book")]
        [Authorize(Roles = Constants.CUSTOMER_ROLE_NAME)]
        public async Task<IActionResult> BookEvent([FromBody] BookEventDto BookDto)
        {
            var result = await _ticketService.BookEvent(BookDto);
            return HandleResponse(result);
        }

        [HttpGet("my-bookings")]
        [Authorize(Roles = Constants.CUSTOMER_ROLE_NAME)]
        public async Task<IActionResult> GetUserBookings([FromQuery] PaginationModel pagination)
        {
            var result = await _ticketService.GetUserBookings(pagination);
            return HandleResponse(result);
        }

        [HttpDelete("cancel-booking/{ticketId}")]
        [Authorize(Roles = Constants.CUSTOMER_ROLE_NAME)]
        public async Task<IActionResult> CancelBooking(Guid ticketId)
        {
            var result = await _ticketService.CancelBooking(ticketId);
            return HandleResponse(result);
        }

        #endregion
    }
}
