using Evenda.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

public class EventFilterVM
{
    private string _sort = SortColumns.DateTime;
    private string _sortDir = "asc";
    private int _tableSize = 25;

    private static readonly string[] ValidSortColumns =
        [
            SortColumns.DateTime,
            SortColumns.Name,
            SortColumns.Price,
            SortColumns.Tickets,
            SortColumns.Booked
        ];
    private static readonly string[] ValidSortDirections = { "asc", "desc" };
    private static readonly int[] ValidTableSizes = { 10, 25, 50, 75, 100 };

    public string? Search { get; set; }

    [FromQuery(Name = "sort")]
    public string Sort
    {
        get => _sort;
        set => _sort = ValidSortColumns.Contains(value?.ToLower()) ? value.ToLower() : SortColumns.DateTime;
    }

    [FromQuery(Name = "dir")]
    public string SortDir
    {
        get => _sortDir;
        set => _sortDir = ValidSortDirections.Contains(value?.ToLower()) ? value.ToLower() : "asc";
    }

    [FromQuery(Name = "sz")]
    public int TableSize
    {
        get => _tableSize;
        set => _tableSize = ValidTableSizes.Contains(value) ? value : 25;
    }

    [FromQuery(Name = "category")]
    public string Category { get; set; } = "all";

    [FromQuery(Name = "tags")]
    public string Tags { get; set; }

    [FromQuery(Name = "from")]
    public DateOnly? FromDate { get; set; }

    [FromQuery(Name = "to")]
    public DateOnly? ToDate { get; set; }

    [FromQuery(Name = "include-deleted")]
    public bool IncludeDeleted { get; set; }

    [FromQuery(Name = "upcoming-only")]
    public bool UpcomingOnly { get; set; }

    public Guid[] TagIds => Tags?.Split(',')
        .Select(x => Guid.TryParse(x, out Guid parsedGuid) ? parsedGuid : (Guid?)null)
        .Where(x => x.HasValue)
        .Select(x => x.GetValueOrDefault())
        .ToArray() ?? [];
}
