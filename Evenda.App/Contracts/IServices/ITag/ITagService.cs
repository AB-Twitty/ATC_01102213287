using Evenda.App.Dtos.Tag;
using Evenda.App.Models;

namespace Evenda.App.Contracts.IServices.ITag
{
    public interface ITagService
    {
        Task<DataResponse<IList<TagDto>>> GetTags(bool inUse = true);
    }
}
