namespace Evenda.UI.Dtos.Media
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public bool IsThumbnail { get; set; }
        public FileUploadDto File { get; set; }
    }
}
