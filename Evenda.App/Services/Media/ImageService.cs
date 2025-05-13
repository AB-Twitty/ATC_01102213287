using Evenda.App.Contracts.IPersistence.IRepositories;
using Evenda.App.Contracts.IPersistence.IUnitOfWork;
using Evenda.App.Contracts.IServices.IMedia;
using Evenda.App.Dtos.Media;
using Evenda.Domain.Entities.MediaEntities;

namespace Evenda.App.Services.Media
{
    public class ImageService : IImageService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<Image> _imageRepo;

        #endregion

        #region Ctor

        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _imageRepo = _unitOfWork.GetRepository<Image>();
        }

        #endregion

        #region Features

        public async Task SaveEventImages(IList<FileUploadDto> imgFiles, Guid eventId, int thumbnailIdx = 0)
        {
            foreach (var imgFile in imgFiles)
            {
                var img = new Image
                {
                    Name = imgFile.FileName,
                    ContentType = imgFile.ContentType,
                    ImageStream = imgFile.FileStream,
                    EventId = eventId,
                    IsThumbnail = thumbnailIdx == imgFiles.IndexOf(imgFile)
                };

                try
                {
                    await _imageRepo.AddAsync(img);
                }
                catch { }
            }

            await _unitOfWork.SaveChangesAsync();
        }

        #endregion
    }
}
