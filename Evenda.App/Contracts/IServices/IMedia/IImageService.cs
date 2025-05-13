using Evenda.App.Dtos.Media;
using Evenda.Domain.Entities.MediaEntities;

namespace Evenda.App.Contracts.IServices.IMedia
{
    public interface IImageService
    {
        Task SaveEventImages(IList<FileUploadDto> imgFiles, Guid eventId, int thumbnailIdx = 0);
        Task<Image?> GetEventThumbnailImg(Guid eventId);
    }
}
