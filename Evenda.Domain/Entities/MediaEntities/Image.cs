using Evenda.Domain.Base;
using Evenda.Domain.Entities.EventEntities;
using System.Text.Json.Serialization;

namespace Evenda.Domain.Entities.MediaEntities
{
    public class Image : BaseEntity
    {
        public string Path { get; set; }
        public string? Name { get; set; }
        public string Extension { get; set; }
        public bool IsThumbnail { get; set; }
        public Guid EventId { get; set; }

        [JsonIgnore]
        public virtual Event Event { get; set; }
    }
}
