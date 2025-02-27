using FMoneAPI.DTOs;
using FMoneAPI.Models;
using FMoneAPI.Services.NewsService;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FMoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        public NewsController(INewsService newsService)
        {
            _newsService = newsService ?? throw new ArgumentNullException(nameof(_newsService));
        }
        private string GetBase64FromImagePath(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return null;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));

            if (!System.IO.File.Exists(filePath))
                return null;

            byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
            return Convert.ToBase64String(imageBytes);
        }

        [HttpGet]
        public async Task<IActionResult> GetNews()
        {

            var news = await _newsService.GetAllNewsAsync();
            var cetegory = await _newsService.GetAllCategory();
            var newsWithBase64 = news.Select(item =>
            {
                var dict = item.GetType().GetProperties()
                               .ToDictionary(prop => prop.Name, prop => prop.GetValue(item));

                dict["ImageBase64"] = GetBase64FromImagePath(item.ImageUrl); // เพิ่มฟิลด์ใหม่
                dict["Categories"] = cetegory.Where(w => w.NewsId == item.Id).Select(c => new
                {
                    c.NewsCategory.Id,
                    c.NewsCategory.Name
                }).ToList();

                return dict;
            });
            return Ok(new { status = 200, data = newsWithBase64 });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            var cetegory = await _newsService.GetAllCategory();
            if (news == null) return NotFound();

            var dict = news.GetType().GetProperties()
                      .ToDictionary(prop => prop.Name, prop => prop.GetValue(news));

            dict["ImageBase64"] = GetBase64FromImagePath(news.ImageUrl);
            dict["Categories"] = cetegory.Where(w => w.NewsId == id).Select(c => new
            {
                c.NewsCategory.Id,
                c.NewsCategory.Name
            }).ToList();

            return Ok(new { status = 200, data = dict });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews([FromForm] NewsDTO newsDto, [FromForm] IFormFile file, [FromForm] List<string> categoryIds)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is required" });

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "news");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            newsDto.ImageUrl = $"/news/{fileName}";

            List<int> categoryIdsInt = new();

            foreach (var id in categoryIds)
            {
                if (int.TryParse(id, out int result))
                {
                    categoryIdsInt.Add(result);
                }
            }

            var createdNews = await _newsService.CreateNewsAsync(newsDto, categoryIdsInt);
            return CreatedAtAction(nameof(GetNewsById), new { id = createdNews.Id }, createdNews);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromForm] NewsDTO newsDto, [FromForm] IFormFile? file, [FromForm] List<string> categoryIds)
        {
            var getNews = await _newsService.GetNewsByIdAsync(id);
            if (getNews == null)
                return NotFound(new { message = "News not found" });
            // อัปโหลดไฟล์ใหม่
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "news");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ลบไฟล์เดิม
                if (!string.IsNullOrEmpty(getNews.ImageUrl))
                {
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", getNews.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                getNews.ImageUrl = $"/news/{fileName}";
            }
            getNews.Title = newsDto.Title;
            getNews.LinkUrl = newsDto.LinkUrl;
            getNews.Tags = newsDto.Tags;
            getNews.UpdatedBy = newsDto.UpdatedBy;

            List<int> categoryIdsInt = new();

            foreach (var idPut in categoryIds)
            {
                if (int.TryParse(idPut, out int result))
                {
                    categoryIdsInt.Add(result);
                }
            }

            var updatedNews = await _newsService.UpdateNewsAsync(id, getNews, categoryIdsInt);
            if (updatedNews == null) return NotFound();
            return Ok(updatedNews);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
       
            var getNews = await _newsService.GetNewsByIdAsync(id);
            if (getNews == null)
                return NotFound(new { message = "News not found" });

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "news");
            string filePath = Path.Combine(uploadsFolder, Path.GetFileName(getNews.ImageUrl));

            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "news");
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            var isDeleted = await _newsService.DeleteNewsAsync(id);
            if (!isDeleted) return NotFound();
            return NoContent();
        }
    }
}
