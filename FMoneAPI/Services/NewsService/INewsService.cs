using FMoneAPI.DTOs;
using FMoneAPI.Models;

namespace FMoneAPI.Services.NewsService
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDTO>> GetAllNewsAsync();
        Task<NewsDTO?> GetNewsByIdAsync(int id);
        Task<NewsDTO> CreateNewsAsync(NewsDTO newsDto, List<int> categoryIds);
        Task<NewsDTO?> UpdateNewsAsync(int id, NewsDTO newsDto, List<int> categoryIds);
        Task<bool> DeleteNewsAsync(int id);
        Task<IEnumerable<NewsCategoryMapping>> GetAllCategory();
        Task<NewsCategoryMapping> GetGetByIdlCategory(int id);
        Task UpdateSortOrderAsync(NewsSortRequestDTO request);
    }
}
