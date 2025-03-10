namespace FMoneAPI.Models
{
    public class Banner
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? Link { get; set; }
        public string? UpdateBy { get; set; }
        public int? SortOrder { get; set; }
        public int? Status { get; set; }
        public int? CTR { get; set; }
    }
}
