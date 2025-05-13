namespace Evenda.UI.Helpers
{
    public partial class PagedList<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int FilterCount { get; set; }
        public int TotalPages { get; set; }
        public IList<T> Items { get; set; } = [];
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
