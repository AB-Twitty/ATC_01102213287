using Evenda.UI.Dtos.Event;
using Evenda.UI.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evenda.UI.Models.EventVM
{
    public class EventsListVM
    {
        public PagedList<EventDto> Events { get; set; } = new PagedList<EventDto>();
        public EventFilterVM Filter { get; set; } = new EventFilterVM();


        public IEnumerable<SelectListItem> SortingSelectItems =
        [
            new SelectListItem("Recent", $"{SortColumns.DateTime}|desc"),
            new SelectListItem("Price: Low to High", $"{SortColumns.Price}|asc"),
            new SelectListItem("Price: High to Low", $"{SortColumns.Price}|desc"),
            new SelectListItem("Most Booked", $"{SortColumns.Booked}|desc"),
        ];

        public IEnumerable<SelectListItem> TableSizeSelect =
        [
            new SelectListItem{ Text="10", Value="10"},
            new SelectListItem{ Text="25", Value="25"},
            new SelectListItem{ Text="50", Value="50"},
            new SelectListItem{ Text="75", Value="75"},
            new SelectListItem{ Text="100", Value="100"}
        ];
    }
}
