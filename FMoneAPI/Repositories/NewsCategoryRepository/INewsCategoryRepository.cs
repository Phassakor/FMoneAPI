using FMoneAPI.DTOs;
using FMoneAPI.Models;

namespace FMoneAPI.Repositories.NewsCategoryRepository
{
    public interface INewsCategoryRepository
    {
        Task<IEnumerable<NewsCategory>> GetAllAsync();
        Task<NewsCategory?> GetByIdAsync(int id);
        Task<NewsCategory> CreateAsync(NewsCategory category);
        Task<NewsCategory?> UpdateAsync(int id, NewsCategoryDTO categoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
