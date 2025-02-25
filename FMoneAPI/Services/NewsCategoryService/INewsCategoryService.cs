using FMoneAPI.DTOs;

namespace FMoneAPI.Services.NewsCategoryService
{
    public interface INewsCategoryService
    {
        Task<IEnumerable<NewsCategoryDTO>> GetAllCategoriesAsync();
        Task<NewsCategoryDTO?> GetCategoryByIdAsync(int id);
        Task<NewsCategoryDTO> CreateCategoryAsync(NewsCategoryDTO categoryDto);
        Task<NewsCategoryDTO?> UpdateCategoryAsync(int id, NewsCategoryDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
