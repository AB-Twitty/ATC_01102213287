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

        public async Task SaveEventImages(IList<FileUploadDto> imgFiles, Guid eventId, string? thumbnailKey = null)
        {
            int.TryParse(thumbnailKey, out int thumbnailIdx);

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
        }

        public async Task<Image?> GetEventThumbnailImg(Guid eventId)
        {
            return await _imageRepo.FirstOrDefaultAsync(
                        predicate: e => e.EventId == eventId && e.IsThumbnail
                    );
        }

        public async Task DeleteEventImages(IList<Guid> DeletedImgIds)
        {
            var images = await _imageRepo.FindAsync(
                predicate: e => DeletedImgIds.Contains(e.Id)
            );

            if (images?.Any() ?? false)
            {
                foreach (var img in images)
                {
                    _imageRepo.Delete(img);
                }
            }
        }

        public async Task SetImageAsThumbnail(Guid imgId)
        {
            var img = await _imageRepo.GetByIdAsync(imgId);
            if (img != null)
            {
                img.IsThumbnail = true;
                _imageRepo.Update(img);
            }
        }

        #endregion
    }
}
