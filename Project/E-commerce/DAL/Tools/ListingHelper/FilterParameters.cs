namespace DAL.Tools.ListingHelper
{
    public class FilterParameters
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; } = false;

        // For column-specific searches
        public Dictionary<string, string>? ColumnSearchTerms { get; set; }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
