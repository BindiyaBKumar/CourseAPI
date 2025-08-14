namespace CourseAppAPI.DTO
{
    public class FilterDTO
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string? status { get; set; }
        public string? sort { get; set; }
        public string? tutor { get; set; }
        public string? queryString { get; set; }

    }
}
