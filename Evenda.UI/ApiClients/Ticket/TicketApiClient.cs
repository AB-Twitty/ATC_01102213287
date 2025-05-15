using Evenda.UI.Contracts.IApiClients.ITicket;
using Evenda.UI.Dtos.Ticket;
using Evenda.UI.Helpers;

namespace Evenda.UI.ApiClients.Ticket
{
    public class TicketApiClient : ApiClient, ITicketApiClient
    {
        #region Fields

        #endregion

        #region Ctor

        public TicketApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }

        #endregion

        #region Requests

        public async Task<Guid> BookEventAsync(BookEventDto bookDto)
        {
            var response = await PostAsync<Guid, BookEventDto>(
                url: ApiEndPoints.BOOK_EVENT,
                data: bookDto
            );
            return response.Data;
        }

        public async Task<PagedList<BookingDto>> GetMyBookings(int page = 1, int pageSize = 15)
        {
            var response = await GetAsync<PagedList<BookingDto>>(
                url: ApiEndPoints.GET_MY_BOOKINGS + $"?page={page}&pageSize={pageSize}"
            );
            return response.Data;
        }

        #endregion
    }
}
