namespace Evenda.UI.Dtos.Media
{
    public class FileUploadDto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Byte[] FileStream { get; set; }
    }
}
