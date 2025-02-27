namespace FMoneAPI.DTOs
{
    public class NewsCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int? NewsCount { get; set; }
    }
}
