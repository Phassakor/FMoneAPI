using AutoMapper;
using FMoneAPI.Data;
using FMoneAPI.DTOs;
using FMoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FMoneAPI.Repositories.NewsCategoryRepository
{
    public class NewsCategoryRepository : INewsCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NewsCategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<NewsCategory>> GetAllAsync()
        {
            return await _context.Newscategory
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<NewsCategory?> GetByIdAsync(int id)
        {
            return await _context.Newscategory.FindAsync(id);
        }

        public async Task<NewsCategory> CreateAsync(NewsCategory category)
        {
            _context.Newscategory.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<NewsCategory?> UpdateAsync(int id, NewsCategoryDTO categoryDto)
        {
            var category = await _context.Newscategory.FindAsync(id);
            if (category == null) return null;

            category.Name = categoryDto.Name;
            category.Status = categoryDto.Status;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Newscategory.FindAsync(id);
            if (category == null) return false;

            _context.Newscategory.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<NewsCategoryMapping>> GetAllNewsCategoryMapping()
        {
            return await _context.Newscategorymapping.ToListAsync();
        }
    }
}
