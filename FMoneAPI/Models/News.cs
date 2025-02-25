namespace FMoneAPI.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? LinkUrl { get; set; }
        public string? Tags { get; set; }
        public int SortOrder { get; set; } = 0;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;

        // Many-to-Many Relationship
        public ICollection<NewsCategoryMapping> CategoryMappings { get; set; } = new List<NewsCategoryMapping>();
    }

    public class NewsCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Many-to-Many Relationship
        public ICollection<NewsCategoryMapping> NewsMappings { get; set; } = new List<NewsCategoryMapping>();
    }

    public class NewsCategoryMapping
    {
        public int NewsId { get; set; }
        public News News { get; set; } = null!;

        public int CategoryId { get; set; }
        public NewsCategory NewsCategory { get; set; } = null!;
    }
}
