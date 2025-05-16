using Evenda.UI.Dtos.Event;

namespace Evenda.UI.Models
{
    public class HomeVM
    {
        public IList<EventDto> LatestEvents { get; set; }
        public IList<EventDto> UpcomingEvents { get; set; }
        public IList<EventDto> MostBookedUpcomingEvents { get; set; }
    }
}
