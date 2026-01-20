using InventoryManagement.API.DTOs;
using InventoryManagement.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        [Authorize]  
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [Authorize]  
        public async Task<IActionResult> GetById(int id)
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]  
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            var category = await categoryService.AddCategoryAsync(dto);
            return Ok(category);
        }

      
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]  
        public async Task<IActionResult> Update(int id, CategoryDto dto)
        {
            var updated = await categoryService.UpdateCategoryAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]  
        public async Task<IActionResult> Delete(int id)
        {
            var result = await categoryService.DeleteCategoryAsync(id);
            if (!result) return NotFound();
            return Ok("Category deleted successfully");
        }
    }
}
