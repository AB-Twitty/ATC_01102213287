namespace Evenda.UI.Models
{
    public class PaginationVM
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public IDictionary<string, string?> RouteParams { get; set; } = new Dictionary<string, string?>();
    }
}
