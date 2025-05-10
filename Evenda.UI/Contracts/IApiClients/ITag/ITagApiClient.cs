using Evenda.UI.Dtos.Tag;

namespace Evenda.UI.Contracts.IApiClients.ITag
{
    public interface ITagApiClient
    {
        Task<IList<TagDto>> SendGetTagsReq();
        Task<IList<TagDto>> SendGetTagsInUseReq();
    }
}
