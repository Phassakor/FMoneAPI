using Azure;
using FMoneAPI.DTOs;
using FMoneAPI.Models;
using FMoneAPI.Services.BannerService;
using FMoneAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FMoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;
        private readonly IUserService _userService;
        public BannerController(IBannerService bannerService, IUserService userService)
        {
            _bannerService = bannerService ?? throw new ArgumentNullException(nameof(bannerService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
        public async Task<IActionResult> GetAll()
        {
            var banners = await _bannerService.GetBanners();

            var response = banners.Select(b => new
            {
                b.ID,
                b.Title,
                b.Link,
                b.UpdateBy,
                b.UpdateDate,
                b.Status,
                b.CTR,
                ImageUrl = b.ImageUrl,
                ImageBase64 = GetBase64FromImagePath(b.ImageUrl)

            });

            return Ok(new { status = 200, data = response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var banner = await _bannerService.GetBannerById(id);
            if (banner == null)
                return NotFound(new { message = "Banner not found" });

            var response = new
            {
                banner.ID,
                banner.Title,
                banner.Link,
                banner.UpdateBy,
                banner.UpdateDate,
                banner.Status,
                banner.CTR,
                ImageUrl = banner.ImageUrl,
                ImageBase64 = GetBase64FromImagePath(banner.ImageUrl) // แปลงเป็น Base64
            };

            return Ok(new { status = 200, data = response });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IFormFile file, [FromForm] string title, [FromForm] string link, [FromForm] string userBy)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is required" });

            if (string.IsNullOrEmpty(title))
                return BadRequest(new { message = "Title is required" });


            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var banner = new Banner
            {
                Title = title,
                ImageUrl = $"/uploads/{fileName}",
                CreateDate = DateTime.Now,
                UpdateBy = userBy,
                Link = link
            };

            await _bannerService.AddBanner(banner);

            return CreatedAtAction(nameof(GetById), new { id = banner.ID }, banner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] IFormFile? file, [FromForm] string title, [FromForm] string link, [FromForm] string userId)
        {
            var banner = await _bannerService.GetBannerById(id);
            var getUser = await _userService.GetUserById(int.Parse(userId));
            if (banner == null)
                return NotFound(new { message = "Banner not found" });
            if (getUser == null)
                return NotFound(new { message = "User not found" });
            // อัปโหลดไฟล์ใหม่
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // ลบไฟล์เดิม
                if (!string.IsNullOrEmpty(banner.ImageUrl))
                {
                    string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", banner.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                banner.ImageUrl = $"/uploads/{fileName}";
            }

            banner.Title = title ?? banner.Title;
            banner.Link = link ?? banner.Link;
            banner.UpdateBy = getUser.Fname ?? banner.UpdateBy;

            await _bannerService.UpdateBanner(id, banner);
            return Ok(new { status = 200, data = banner });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _bannerService.GetBannerById(id);
            if (banner == null) return NotFound();

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            string filePath = Path.Combine(uploadsFolder, Path.GetFileName(banner.ImageUrl));

            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            await _bannerService.DeleteBanner(id);
            return NoContent();
        }

        [HttpPut("sortOrder")]
        public async Task<IActionResult> UpdateSortOrder([FromBody] BannerSortRequestDTO request)
        {
            try
            {
                await _bannerService.UpdateSortOrderAsync(request);
                return Ok(new { status = 200, message = "Sort order updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isActive)
        {
            var updatedBanner = await _bannerService.UpdateBannerStatus(id, isActive);
            if (updatedBanner == null) return NotFound();

            return Ok(new { status = 200, message = "Status updated successfully" });
        }

        [HttpPut("{id}/ctr")]
        public async Task<IActionResult> UpdateCTR(int id)
        {
            var updatedBanner = await _bannerService.UpdateBannerCTR(id);
            if (updatedBanner == null) return NotFound();

            return Ok(new { status = 200, message = "CTR updated successfully" });
        }
    }
}
