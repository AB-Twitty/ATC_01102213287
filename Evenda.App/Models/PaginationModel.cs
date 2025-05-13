using Microsoft.AspNetCore.Mvc;

namespace Evenda.App.Models
{
    public class PaginationModel
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;
        [FromQuery(Name = "sz")]
        public int PageSize { get; set; } = 12;
        [FromQuery(Name = "sort")]
        public string? Sort { get; set; }
        [FromQuery(Name = "dir")]
        public string SortDir { get; set; } = "asc";

        public bool IsOrderDesc() => SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase);
    }
}
