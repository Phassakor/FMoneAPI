using Azure;
using FMoneAPI.DTOs;
using FMoneAPI.Services.BannerService;
using FMoneAPI.Services.NewsCategoryService;
using Microsoft.AspNetCore.Mvc;

namespace FMoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsCategoryController : ControllerBase
    {
        private readonly INewsCategoryService _newsCategoryService;
        public NewsCategoryController(INewsCategoryService newsCategoryService)
        {
            _newsCategoryService = newsCategoryService ?? throw new ArgumentNullException(nameof(newsCategoryService));
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _newsCategoryService.GetAllCategoriesAsync();
            return Ok(new { status = 200, data = categories });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _newsCategoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(new { status = 200, data = category });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] NewsCategoryDTO categoryDto)
        {
            var createdCategory = await _newsCategoryService.CreateCategoryAsync(categoryDto);
            return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] NewsCategoryDTO categoryDto)
        {
            var updatedCategory = await _newsCategoryService.UpdateCategoryAsync(id, categoryDto);
            if (updatedCategory == null) return NotFound();
            return Ok(new { status = 200, message = updatedCategory });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var isDeleted = await _newsCategoryService.DeleteCategoryAsync(id);
            if (!isDeleted) return NotFound();
            return NoContent();
        }
    }
}
