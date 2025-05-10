using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;

namespace Evenda.UI.Models.EventVM
{
    public class EventCardsListVM
    {
        public PagedList<EventDto> Events { get; set; } = new PagedList<EventDto>();
        public EventFilterDto Filter { get; set; } = new EventFilterDto();
    }
}
