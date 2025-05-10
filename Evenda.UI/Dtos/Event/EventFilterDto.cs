using Microsoft.AspNetCore.Mvc;

namespace Evenda.UI.Dtos.Event
{
    public class EventFilterDto
    {
        [FromQuery(Name = "category")]
        public string Category { get; set; }

        [FromQuery(Name = "tag")]
        public string Tag { get; set; }

        [FromQuery(Name = "from")]
        public DateTime? FromDate { get; set; }

        [FromQuery(Name = "to")]
        public DateTime? ToDate { get; set; }
    }
}
