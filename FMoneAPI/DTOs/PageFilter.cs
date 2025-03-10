namespace FMoneAPI.DTOs
{
    public class PageFilter
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
    }
}
