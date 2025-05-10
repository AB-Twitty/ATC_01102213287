namespace Evenda.App.Utils
{
    public partial class PagedList<T>
    {
        public PagedList(IList<T> source, int pageIndex, int pageSize, int totalCount, int? filterCount = null)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            FilterCount = totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            Items = source;
        }

        public PagedList() { }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int FilterCount { get; set; }
        public int TotalPages { get; set; }
        public IList<T> Items { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
