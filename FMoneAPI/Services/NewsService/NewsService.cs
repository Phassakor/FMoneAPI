using FMoneAPI.DTOs;
using FMoneAPI.Models;
using FMoneAPI.Repositories.NewsRepository;

namespace FMoneAPI.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<IEnumerable<NewsDTO>> GetAllNewsAsync()
        {
            var news = await _newsRepository.GetAllAsync();
            return news.Select(n => new NewsDTO
            {
                Id = n.Id,
                Title = n.Title,
                ImageUrl = n.ImageUrl,
                LinkUrl = n.LinkUrl,
                Tags = n.Tags,
                SortOrder = n.SortOrder,
                UpdatedAt = n.UpdatedAt,
                UpdatedBy = n.UpdatedBy
            });
        }

        public async Task<NewsDTO?> GetNewsByIdAsync(int id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news == null) return null;

            return new NewsDTO
            {
                Id = news.Id,
                Title = news.Title,
                ImageUrl = news.ImageUrl,
                LinkUrl = news.LinkUrl,
                Tags = news.Tags,
                SortOrder = news.SortOrder,
                UpdatedAt = news.UpdatedAt,
                UpdatedBy = news.UpdatedBy
            };
        }

        public async Task<NewsDTO> CreateNewsAsync(NewsDTO newsDto, List<int> categoryIds)
        {
            var createdNews = await _newsRepository.CreateNewsAsync(newsDto, categoryIds);
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

        public async Task<NewsDTO?> UpdateNewsAsync(int id, NewsDTO newsDto, List<int> categoryIds)
        {
            var updatedNews = await _newsRepository.UpdateNewsAsync(id, newsDto, categoryIds);
            if (updatedNews == null) return null;

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

        public async Task<bool> DeleteNewsAsync(int id)
        {
            return await _newsRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<NewsCategoryMapping>> GetAllCategory()
        {
            return await _newsRepository.GetAllCategory();
        }
        public async Task<NewsCategoryMapping> GetGetByIdlCategory(int id)
        {
            return await _newsRepository.GetGetByIdlCategory(id);
        }
        public async Task UpdateSortOrderAsync(NewsSortRequestDTO request)
        {
            if (request.News == null || request.News.Count == 0)
            {
                throw new ArgumentException("News list cannot be empty");
            }
            await _newsRepository.UpdateSortOrderAsync(request.News);
        }
    }
}
