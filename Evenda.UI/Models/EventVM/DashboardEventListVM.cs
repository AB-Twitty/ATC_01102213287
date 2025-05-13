using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evenda.UI.Models.EventVM
{
    public class DashboardEventListVM
    {
        public PagedList<EventDto> Events { get; set; } = new PagedList<EventDto>();
        public EventFilterVM Filter { get; set; } = new EventFilterVM();

        public IEnumerable<SelectListItem> TableSizeSelect = new List<SelectListItem>
        {
            new SelectListItem{ Text="10", Value="10"},
            new SelectListItem{ Text="25", Value="25"},
            new SelectListItem{ Text="50", Value="50"},
            new SelectListItem{ Text="75", Value="75"},
            new SelectListItem{ Text="100", Value="100"}
        };
    }
}
