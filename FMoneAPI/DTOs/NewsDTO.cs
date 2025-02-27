namespace FMoneAPI.DTOs
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? LinkUrl { get; set; }
        public string? Tags { get; set; }
        public int SortOrder { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
