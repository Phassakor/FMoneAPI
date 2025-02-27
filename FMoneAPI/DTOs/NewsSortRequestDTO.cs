namespace FMoneAPI.DTOs
{
    public class NewsSortRequestDTO
    {
        public List<NewsSortOrderDto> News { get; set; } = new();
    }

    public class NewsSortOrderDto
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
    }
}
