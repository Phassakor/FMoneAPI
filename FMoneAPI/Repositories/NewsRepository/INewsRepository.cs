using FMoneAPI.DTOs;
using FMoneAPI.Models;

namespace FMoneAPI.Repositories.NewsRepository
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetAllAsync(bool sortDescending = false);
        Task<News?> GetByIdAsync(int id);
        Task<News> CreateAsync(News news);
        Task<NewsDTO> CreateNewsAsync(NewsDTO newsDto, List<int> categoryIds);
        Task<News?> UpdateAsync(int id, NewsDTO newsDto);
        Task<NewsDTO?> UpdateNewsAsync(int id, NewsDTO newsDto, List<int> categoryIds);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<NewsCategoryMapping>> GetAllCategory();
        Task<NewsCategoryMapping> GetGetByIdlCategory(int id);
        Task UpdateSortOrderAsync(List<NewsSortOrderDto> news);
    }
}
