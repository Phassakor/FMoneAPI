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
    }
}
