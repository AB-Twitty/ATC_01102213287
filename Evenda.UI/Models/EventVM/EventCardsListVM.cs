using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evenda.UI.Models.EventVM
{
    public class EventCardsListVM
    {
        public PagedList<EventDto> Events { get; set; } = new PagedList<EventDto>();
        public EventFilterDto Filter { get; set; }

        // Filteration Data
        public IEnumerable<SelectListItem> TagsSelectItems { get; set; }
    }
}
