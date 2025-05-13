using Evenda.App.Dtos.Tag;
using Evenda.App.Models;
using TagEntity = Evenda.Domain.Entities.TagEntities.Tag;

namespace Evenda.App.Contracts.IServices.ITag
{
    public interface ITagService
    {
        Task<DataResponse<IList<TagDto>>> GetTags(bool inUse = true);
        Task<IList<TagEntity>> ProcessTagsAsStrings(IList<string> tags, bool returnOnlyNewTags = false, bool persistNewTags = true);
    }
}
