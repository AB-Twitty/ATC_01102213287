namespace Evenda.UI.Models
{
    public class SortingLinkModel
    {
        public string Sort { get; set; }
        public string SortDir { get; set; }
        public bool IsActive { get; set; }
        public IDictionary<string, string?> RouteParams { get; set; }
    }
}
