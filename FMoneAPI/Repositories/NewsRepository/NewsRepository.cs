using AutoMapper;
using FMoneAPI.Data;
using FMoneAPI.DTOs;
using FMoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FMoneAPI.Repositories.NewsRepository
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NewsRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<News>> GetAllAsync(bool sortDescending = false)
        {
            return sortDescending
               ? await _context.News.OrderByDescending(n => n.SortOrder).ToListAsync()
               : await _context.News.OrderBy(n => n.SortOrder).ToListAsync();
        }

        public async Task<News?> GetByIdAsync(int id)
        {
            return await _context.News.FindAsync(id);
        }

        public async Task<News> CreateAsync(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            return news;
        }

        public async Task<NewsDTO> CreateNewsAsync(NewsDTO newsDto, List<int> categoryIds)
        {
            // สร้างข่าวใหม่
            var news = new News
            {
                Title = newsDto.Title,
                ImageUrl = newsDto.ImageUrl,
                LinkUrl = newsDto.LinkUrl,
                Tags = newsDto.Tags,
                SortOrder = newsDto.SortOrder,
                UpdatedAt = newsDto.UpdatedAt,
                UpdatedBy = newsDto.UpdatedBy
            };

            var createdNews = await CreateAsync(news);

            // เชื่อมโยงข่าวกับหมวดหมู่
            foreach (var categoryId in categoryIds)
            {
                var mapping = new NewsCategoryMapping
                {
                    NewsId = createdNews.Id,
                    CategoryId = categoryId
                };

                _context.Newscategorymapping.Add(mapping);
            }

            await _context.SaveChangesAsync();

            return new NewsDTO
            {
                Id = createdNews.Id,
                Title = createdNews.Title,
                ImageUrl = createdNews.ImageUrl,
                LinkUrl = createdNews.LinkUrl,
                Tags = createdNews.Tags,
                SortOrder = createdNews.SortOrder,
                UpdatedAt = createdNews.UpdatedAt,
                UpdatedBy = createdNews.UpdatedBy
            };
        }

        public async Task<News?> UpdateAsync(int id, NewsDTO newsDto)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return null;

            news.Title = newsDto.Title;
            news.ImageUrl = newsDto.ImageUrl;
            news.LinkUrl = newsDto.LinkUrl;
            news.Tags = newsDto.Tags;
            news.UpdatedBy = newsDto.UpdatedBy;

            await _context.SaveChangesAsync();
            return news;
        }
        public async Task<NewsDTO?> UpdateNewsAsync(int id, NewsDTO newsDto, List<int> categoryIds)
        {
            var updatedNews = await UpdateAsync(id, newsDto);
            if (updatedNews == null) return null;

            // ลบการเชื่อมโยงเดิมใน NewsCategoryMappings
            var existingMappings = _context.Newscategorymapping.Where(m => m.NewsId == id);
            _context.Newscategorymapping.RemoveRange(existingMappings);

            // เพิ่มการเชื่อมโยงใหม่
            foreach (var categoryId in categoryIds)
            {
                var mapping = new NewsCategoryMapping
                {
                    NewsId = updatedNews.Id,
                    CategoryId = categoryId
                };

                _context.Newscategorymapping.Add(mapping);
            }

            await _context.SaveChangesAsync();

            return new NewsDTO
            {
                Id = updatedNews.Id,
                Title = updatedNews.Title,
                ImageUrl = updatedNews.ImageUrl,
                LinkUrl = updatedNews.LinkUrl,
                Tags = updatedNews.Tags,
                SortOrder = updatedNews.SortOrder,
                UpdatedAt = updatedNews.UpdatedAt,
                UpdatedBy = updatedNews.UpdatedBy
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return false;

            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteNewsAsync(int id)
        {
            // ลบการเชื่อมโยงใน NewsCategoryMappings ก่อน
            var mappings = _context.Newscategorymapping.Where(m => m.NewsId == id);
            _context.Newscategorymapping.RemoveRange(mappings);

            var isDeleted = await DeleteAsync(id);
            return isDeleted;
        }

        public async Task<IEnumerable<NewsCategoryMapping>> GetAllCategory()
        {
            var newsList = await _context.News
            .Include(n => n.CategoryMappings) // Include Join Table
            .ThenInclude(nc => nc.NewsCategory) // Include Category
            .ToListAsync();

            if (newsList == null) return null;

            // ดึงเฉพาะ CategoryMappings จาก News
            var categoryMappings = newsList.SelectMany(n => n.CategoryMappings);

            return categoryMappings;
        }

        public async Task<NewsCategoryMapping> GetGetByIdlCategory(int id)
        {
            var news = await _context.News
        .Include(n => n.CategoryMappings) // Include Join Table
        .ThenInclude(nc => nc.NewsCategory) // Include Category
        .FirstOrDefaultAsync(n => n.Id == id);

            if (news == null) return null;

            // ดึง CategoryMappings จาก News
            var categoryMapping = news.CategoryMappings.FirstOrDefault(); // หรือเลือก Mapping อื่นๆ ตามที่ต้องการ

            return categoryMapping;
        }
    }
}
