using Evenda.App.Dtos.Media;

namespace Evenda.App.Contracts.IServices.IMedia
{
    public interface IImageService
    {
        Task SaveEventImages(IList<FileUploadDto> imgFiles, Guid eventId, int thumbnailIdx = 0);
    }
}
