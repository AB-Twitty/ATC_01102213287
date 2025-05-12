using Evenda.UI.Dtos.Media;
using Evenda.UI.Models.EventVM;

namespace Evenda.UI.Dtos.Event
{
    public class CreateEventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public IList<string> Tags { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }

        public int TicketsQty { get; set; }
        public int ThumbnailIdx { get; set; }
        public IList<FileUploadDto> Images { get; set; } = new List<FileUploadDto>();

        public CreateEventDto(CreateEventVM @createVM)
        {
            Name = @createVM.Name;
            Description = @createVM.Description;
            Price = @createVM.Price;
            Category = @createVM.Category;

            Tags = @createVM.StringTags?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim()).ToList() ?? new List<string>();

            Country = @createVM.Country;
            City = @createVM.City;
            Venue = @createVM.Venue;

            TicketsQty = @createVM.TicketsQty;
            ThumbnailIdx = @createVM.ThumbnailIdx;

            ReadImagesFromFiles(@createVM.Images);
        }

        private void ReadImagesFromFiles(IList<IFormFile> imageFiles)
        {
            var ms = new MemoryStream();

            foreach (var img in imageFiles)
            {
                try
                {
                    img.CopyTo(ms);

                    var file = new FileUploadDto
                    {
                        FileName = img.FileName,
                        ContentType = img.ContentType,
                        FileStream = ms.ToArray()
                    };

                    Images.Add(file);
                }
                catch { }
            }

            ms.Close();
        }
    }
}
