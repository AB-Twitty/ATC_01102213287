using Evenda.App.Dtos.Media;
using Evenda.Domain.Entities.MediaEntities;

namespace Evenda.App.Contracts.IServices.IMedia
{
    public interface IImageService
    {
        Task SaveEventImages(IList<FileUploadDto> imgFiles, Guid eventId, string? thumbnailKey = null);
        Task DeleteEventImages(IList<Guid> DeletedImgIds);
        Task SetImageAsThumbnail(Guid imgId);
        Task<Image?> GetEventThumbnailImg(Guid eventId);
    }
}
