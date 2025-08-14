namespace CourseAppAPI.Models
{
    public class PaginatedResponse<CourseDTO>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public IEnumerable<CourseDTO> Items { get; set; }
    }
}
