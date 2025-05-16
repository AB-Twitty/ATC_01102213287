namespace Evenda.UI.Dtos.Event
{
    public class EventFilterDto
    {
        public bool UpcomingOnly { get; set; } = true;
        public bool GetDeletedEvents { get; set; } = false;

        public string? Search { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }

        public Guid[] TagIds => Tags?.Split(',')
            .Select(x => Guid.TryParse(x, out Guid parsedGuid) ? parsedGuid : (Guid?)null)
            .Where(x => x.HasValue)
            .Select(x => x.GetValueOrDefault())
            .ToArray() ?? [];

        public EventFilterDto()
        {

        }

        public EventFilterDto(EventFilterVM filterVM)
        {
            Category = filterVM.Category == "all" ? null : filterVM.Category;
            Tags = filterVM.Tags;
            FromDate = filterVM.FromDate;
            ToDate = filterVM.ToDate;

            Search = filterVM.Search;

            UpcomingOnly = filterVM.UpcomingOnly;
            GetDeletedEvents = filterVM.IncludeDeleted;
        }
    }
}
