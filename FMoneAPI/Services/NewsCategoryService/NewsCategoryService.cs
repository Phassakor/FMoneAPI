using AutoMapper;
using FMoneAPI.DTOs;
using FMoneAPI.Models;
using FMoneAPI.Repositories.NewsCategoryRepository;

namespace FMoneAPI.Services.NewsCategoryService
{
    public class NewsCategoryService : INewsCategoryService
    {
        private readonly INewsCategoryRepository _newsCategoryRepository;
        private readonly IMapper _mapper;
        public NewsCategoryService(INewsCategoryRepository newsCategoryRepository, IMapper mapper)
        {
            _newsCategoryRepository = newsCategoryRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<NewsCategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _newsCategoryRepository.GetAllAsync();
            var mappingNewsCategory = await _newsCategoryRepository.GetAllNewsCategoryMapping();
            return categories.Select(c => new NewsCategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Status = c.Status,
                NewsCount = mappingNewsCategory.Count(n => n.CategoryId == c.Id)
            });
        }

        public async Task<NewsCategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var category = await _newsCategoryRepository.GetByIdAsync(id);
            var mappingNewsCategory = await _newsCategoryRepository.GetAllNewsCategoryMapping();
            if (category == null) return null;

            return new NewsCategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Status = category.Status,
                NewsCount = mappingNewsCategory.Count(n => n.CategoryId == category.Id)
            };
        }

        public async Task<NewsCategoryDTO> CreateCategoryAsync(NewsCategoryDTO categoryDto)
        {
            var category = new NewsCategory
            {
                Name = categoryDto.Name,
                Status = categoryDto.Status
            };

            var createdCategory = await _newsCategoryRepository.CreateAsync(category);
            return new NewsCategoryDTO
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Status = createdCategory.Status
            };
        }

        public async Task<NewsCategoryDTO?> UpdateCategoryAsync(int id, NewsCategoryDTO categoryDto)
        {
            var updatedCategory = await _newsCategoryRepository.UpdateAsync(id, categoryDto);
            if (updatedCategory == null) return null;

            return new NewsCategoryDTO
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                Status = updatedCategory.Status
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _newsCategoryRepository.DeleteAsync(id);
        }
    }
}
