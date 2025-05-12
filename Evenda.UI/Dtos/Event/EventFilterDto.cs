using Microsoft.AspNetCore.Mvc;

namespace Evenda.UI.Dtos.Event
{
    public class EventFilterDto
    {
        [FromQuery(Name = "category")]
        public string Category { get; set; }

        [FromQuery(Name = "tags")]
        public string Tags { get; set; }

        [FromQuery(Name = "from")]
        public DateTime? FromDate { get; set; }

        [FromQuery(Name = "to")]
        public DateTime? ToDate { get; set; }

        public Guid[] TagIds => Tags?.Split(',')
            .Select(x => Guid.TryParse(x, out Guid parsedGuid) ? parsedGuid : (Guid?)null)
            .Where(x => x.HasValue)
            .Select(x => x.GetValueOrDefault())
            .ToArray() ?? [];
    }
}
