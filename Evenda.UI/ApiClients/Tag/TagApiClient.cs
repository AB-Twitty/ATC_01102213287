using Evenda.UI.Contracts.IApiClients.ITag;
using Evenda.UI.Dtos.Tag;
using Evenda.UI.Helpers;

namespace Evenda.UI.ApiClients.Tag
{
    public class TagApiClient : ApiClient, ITagApiClient
    {
        #region Fields

        #endregion

        #region Ctor

        public TagApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {

        }

        #endregion

        #region Requests 

        public async Task<IList<TagDto>> SendGetTagsInUseReq()
        {
            var response = await GetAsync<IList<TagDto>>(string.Format(ApiEndPoints.GET_TAGS, true));
            return response.Data;
        }

        public async Task<IList<TagDto>> SendGetTagsReq()
        {
            var response = await GetAsync<IList<TagDto>>(string.Format(ApiEndPoints.GET_TAGS, false));
            return response.Data;
        }

        #endregion
    }
}
