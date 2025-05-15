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

        #endregion
    }
}
