using Evenda.Domain.Entities.MediaEntities;

namespace Evenda.App.Dtos.Media
{
    public class FileUploadDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Byte[] FileStream { get; set; }

        public FileUploadDto()
        {

        }

        public FileUploadDto(Image @img)
        {
            FileName = img.Name;
            ContentType = img.ContentType;
            FileStream = img.ImageStream;
        }
    }
}
