namespace FMoneAPI.DTOs
{
    public class BannerSortRequestDTO
    {
        public List<BannerSortOrderDto> Banners { get; set; } = new();
    }

    public class BannerSortOrderDto
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
    }
}
